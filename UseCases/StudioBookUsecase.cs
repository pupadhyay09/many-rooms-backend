using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Boundary.Responses;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Helper;
using ManyRoomStudio.Infrastructure;
using ManyRoomStudio.Infrastructure.Enums;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Repository;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManyRoomStudio.UseCases
{
    public class StudioBookUsecase : IStudioBookUsecase
    {
        private readonly IBookingGateway _bookingGateway;
        private readonly IRoomGateway _roomGateway;
        private readonly IEmailSetupDetailsGateway _emailSetupDetailsGateway;
        private readonly IEmailLogGateway _emailLogGateway;
        private readonly IEmailTemplatesGateway _emailTemplatesGateway;
        private readonly IEmailHelper _emailHelper;

        public StudioBookUsecase(IBookingGateway bookingGateway, IRoomGateway roomGateway, IEmailSetupDetailsGateway emailSetupDetailsGateway, IEmailLogGateway emailLogGateway, IEmailTemplatesGateway emailTemplatesGateway, IEmailHelper emailHelper)
        {
            this._bookingGateway = bookingGateway;
            this._roomGateway = roomGateway;
            this._emailSetupDetailsGateway = emailSetupDetailsGateway;
            this._emailLogGateway = emailLogGateway;
            this._emailTemplatesGateway = emailTemplatesGateway;
            this._emailHelper = emailHelper;
        }
        public async Task<StudioBookResponse> ExecuteAsync(BookingRequest request)
        {
            await using var db = ManyRoomStudioContext.Create();
            await using var tran = await db.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = new StudioBookResponse() { Iserror = false, ErrorMessage = "" };

            var bookingitem = request.ToEntity();
            _roomGateway.SetDb(db);
            _bookingGateway.SetDb(db);
            try
            {

                var bookingdata = await _bookingGateway.Get(x => x.IsDelete == false &&
                                                       x.StartDate == request.BookingStartDateTime &&
                                                       x.EndDate == request.BookingEndDateTime &&
                                                       x.RoomID == request.RoomID
                                                       ).ConfigureAwait(false);

                if (bookingdata != null)
                    return new StudioBookResponse() { Iserror = true, ErrorMessage = "The room is already booked for the selected time." };


                var roominfo = await _roomGateway.Get(x => x.IsDelete == false && x.ID == request.RoomID).ConfigureAwait(false);
                if (roominfo == null)
                    return new StudioBookResponse() { Iserror = true, ErrorMessage = "Room details not found." };

                if (request.NumberofPeople > roominfo.Capacity)
                    return new StudioBookResponse() { Iserror = true, ErrorMessage = $"Room capacity is {roominfo.Capacity} people. Please reduce the number of attendees." };

                TimeSpan duration = request.BookingEndDateTime - request.BookingStartDateTime;

                bookingitem.TotalDiscount = 0;
                bookingitem.Status = EStatus.Booked.ToString();
                bookingitem.TotalAmount = Math.Round(roominfo.HourlyPrice * (decimal)duration.TotalHours, 2);
                bookingitem.TotalPaybleAmount = 0;
                bookingitem.VATPercentage = roominfo.VATPercentage;
                if (roominfo.VATPercentage > 0)
                    bookingitem.TotalVATAmount = Math.Round((bookingitem.TotalAmount * roominfo.VATPercentage) / 100);

                var roomitemcreate = await _bookingGateway.Add(bookingitem).ConfigureAwait(false);
                if (roomitemcreate == null)
                    new StudioBookResponse() { Iserror = true, ErrorMessage = "Failed to create booking." };

                await tran.CommitAsync().ConfigureAwait(false);
                response = roomitemcreate.ToResponse();

                try
                {
                    var emailCredentials = await _emailSetupDetailsGateway.Get(x =>
                    x.FromEmail == EmailConstants.FromMail &&
                    x.IsDelete == false).ConfigureAwait(false);

                    if (emailCredentials != null)
                    {
                        var emailTemplate = await _emailTemplatesGateway.Get(x =>
                        x.SystemName == EmailConstants.BookingConfirmation &&
                        x.IsDelete == false).ConfigureAwait(false);

                        if (emailTemplate != null)
                        {
                            var messageBody = emailTemplate.Body;

                            //messageBody = messageBody.Replace("{%Email%}", "");

                            _emailHelper.SetCredentials(emailCredentials);
                            _emailHelper.To("bhavythummar2525@gmail.com");
                            _emailHelper.Subject(emailTemplate.Subject ?? "Booking Confirmation");
                            //_emailHelper.Body(messageBody);
                            _emailHelper.Body(messageBody ?? "test");

                            var isSent = _emailHelper.Send();

                            try
                            {
                                var emailLog = new EmailLog();

                                emailLog.Subject = emailTemplate.Subject;
                                emailLog.Body = messageBody;
                                emailLog.SentDate = DateTime.UtcNow;
                                emailLog.ToEmail = "bhavythummar2525@gmail.com";
                                emailLog.FromEmail = emailTemplate.FromEmail;
                                emailLog.IsSent = isSent;
                                emailLog.IsDelete = false;
                                emailLog.CreatedAt = DateTime.UtcNow;
                                emailLog.CreatedBy = emailTemplate.CreatedBy;

                                await _emailLogGateway.Add(emailLog).ConfigureAwait(false);
                                await tran.CommitAsync().ConfigureAwait(false);
                            }
                            catch (Exception) { }
                        }
                    }
                }
                catch (Exception) { }
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
                return new StudioBookResponse { Iserror = true, ErrorMessage = ex.Message };
            }
            finally
            {
                await db.Database.CloseConnectionAsync().ConfigureAwait(false);
            }
            return response;
        }
    }
}
