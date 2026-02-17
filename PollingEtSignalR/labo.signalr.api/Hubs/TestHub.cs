using System.Diagnostics;
using labo.signalr.api.Data;
using labo.signalr.api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace labo.signalr.api.Hubs
{

    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }
    public class TestHub : Hub
    {

        private readonly ApplicationDbContext _context;
        public TestHub(ApplicationDbContext context)
        {
            _context = context;
        
        }

        public override async Task OnConnectedAsync()
        {
            base.OnConnectedAsync();
            // TODO: Ajouter votre logique
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            await Clients.All.SendAsync("UserCount", UserHandler.ConnectedIds.Count);
            await Clients.Caller.SendAsync("V2", _context.UselessTasks.ToList());
            

            
        }


        
        public async Task AddTask(string taskName)
        {
            // TODO: Ajouter la tache dans la bd
            UselessTask newTask = new UselessTask() { Id = 0, Text = taskName };
            
            _context.UselessTasks.Add(newTask);
            _context.SaveChanges();
            await Clients.All.SendAsync("V2", _context.UselessTasks.ToList());



        



        }


        public async Task Complete(int id) 
        {

            UselessTask taskC = _context.UselessTasks.Find(id);

            taskC.Completed = true;

            _context.SaveChanges();
            await Clients.All.SendAsync("V2", _context.UselessTasks.ToList());



        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            await Clients.All.SendAsync("UserCount", UserHandler.ConnectedIds.Count);
            await base.OnDisconnectedAsync(exception);
        }








    }
}
