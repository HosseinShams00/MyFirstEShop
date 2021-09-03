using System;
using MyFirstEShop.Data;
using MyFirstEShop.Models.DatabaseModels;
using System.Linq;
using System.Collections.Generic;
using MyFirstEShop.Models.ViewModels;

namespace MyFirstEShop.Repositories
{
    public interface ITicketRepository
    {
        int GetTicketCount();
        IEnumerable<TicketViewModel> GetAllTickets(int userId);
        IEnumerable<TicketViewModel> GetAllTicketsForAdmin();
        void SetNewTicket(TicketViewModel ticketViewModel, int userId);
        void SetAnswerForTicket(int userId, int AdminId, int MessageId, string Answer);
        TicketViewModel ReadTicket(int userId, int MessageID);
    }

    public class TicketRepository : ITicketRepository
    {
        private readonly MyDbContext dbContext;
        public TicketRepository(MyDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public int GetTicketCount()
        {
            return dbContext.Tickets
            .Where(i => i.AdminAnswer == null)
            .Count();
        }

        public IEnumerable<TicketViewModel> GetAllTicketsForAdmin()
        {
            return dbContext.Tickets
            .Where(i => i.AdminAnswer == null)
            .Select(i => new TicketViewModel()
            {
                MessageId = i.Id,
                UserId = i.UserId,
                Subject = i.Subject,
                UserQuestion = i.UserQuestion,
                UserSendQuestionDateTime = i.UserSendQuestionDateTime
            }).ToList();
        }

        public IEnumerable<TicketViewModel> GetAllTickets(int userId)
        {
            return dbContext.Tickets
            .Where(Q => Q.UserId == userId)
            .Select(i => new TicketViewModel()
            {
                MessageId = i.Id,
                Subject = i.Subject,
                UserQuestion = i.UserQuestion,
                UserSendQuestionDateTime = i.UserSendQuestionDateTime,
                AdminAnswer = i.AdminAnswer,
                AdminAnswerDateTime = i.AdminAnswerDateTime

            })
            .ToList();
        }

        public void SetNewTicket(TicketViewModel ticketViewModel, int userId)
        {
            dbContext.Tickets.Add(new Ticket()
            {
                UserId = userId,
                Subject = ticketViewModel.Subject,
                UserQuestion = ticketViewModel.UserQuestion,
                UserSendQuestionDateTime = DateTime.Now,
            });
            dbContext.SaveChanges();
        }

        public void SetAnswerForTicket(int userId, int AdminId, int MessageId, string Answer)
        {
            var ticket = dbContext.Tickets
             .SingleOrDefault(i => i.UserId == userId && i.Id == MessageId);

            if (ticket != null)
            {
                ticket.AdminAnswer = Answer;
                ticket.AdminAnswerDateTime = DateTime.Now;
                dbContext.SaveChanges();
            }
        }

        public TicketViewModel ReadTicket(int userId, int MessageID)
        {
            return dbContext.Tickets
            .Where(i => i.UserId == userId && i.Id == MessageID)
            .Select(i => new TicketViewModel()
            {
                MessageId = i.Id,
                UserId = i.UserId,
                Subject = i.Subject,
                UserQuestion = i.UserQuestion,
                UserSendQuestionDateTime = i.UserSendQuestionDateTime,
                AdminAnswer = i.AdminAnswer,
                AdminAnswerDateTime = i.AdminAnswerDateTime,
            }).SingleOrDefault();
        }

        
    }

}
