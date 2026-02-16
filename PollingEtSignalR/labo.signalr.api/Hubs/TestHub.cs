using System.Diagnostics;
using labo.signalr.api.Data;
using labo.signalr.api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace labo.signalr.api.Hubs
{
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
            List<UselessTask> TaskList = await _context.UselessTasks.ToListAsync();


            await Clients.Caller.SendAsync("V2", TaskList);
            
        }


        
        public async Task AddTask(string taskName)
        {
            // TODO: Ajouter la tache dans la bd
            UselessTask newTask = new UselessTask() { Id = 0, Text = taskName };
            

            _context.UselessTasks.Add(newTask);
            await _context.SaveChangesAsync();



        



        }


        public async Task Complete(int id) 
        {

            UselessTask taskC = _context.UselessTasks.Find(id);

            taskC.Completed = true;

            await _context.SaveChangesAsync();



        }








    }
}
