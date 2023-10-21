using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext dataContext, IMapper mapper)
        {
            _mapper = mapper;
            _dataContext = dataContext;
        }

        public void AddMessage(Message message)
        {
            _dataContext.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _dataContext.Messages.Remove(message);
        }

        public async Task<Message> GetMessageAsync(int id)
        {
            var message = await _dataContext.Messages.FindAsync(id);
            return message!;
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams)
        {
            var getMessagesQuery = _dataContext.Messages.OrderByDescending(m => m.MessageSent).AsQueryable();
            getMessagesQuery = messageParams.Container switch
            {
                "Inbox" => getMessagesQuery.Where(message => message.Recipient!.Username == messageParams.Username),
                "Outbox" => getMessagesQuery.Where(message => message.Sender!.Username == messageParams.Username),
                _ => getMessagesQuery.Where(message => message.Recipient!.Username == messageParams.Username
                && message.DateRead == null)
            };

            var messages = getMessagesQuery.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUsername, string recipientUsername)
        {
            var messages = await _dataContext.Messages
                .Include(m => m.Sender).ThenInclude(s => s!.Photos)
                .Include(m => m.Recipient).ThenInclude(r => r!.Photos)
                .Where(message => message.Recipient!.Username == currentUsername && message.Sender!.Username == recipientUsername
                || message.Recipient!.Username == recipientUsername && message.Sender!.Username == currentUsername)
                .OrderBy(message => message.MessageSent).ToListAsync();

            var unreadMessages = messages.Where(message => message.DateRead == DateTime.MinValue && message.RecipientUsername == currentUsername).ToList();
            if (unreadMessages.Any())
            {
                unreadMessages.ForEach(message => message.DateRead = DateTime.Now);
                await _dataContext.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}