import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { UselessTask } from '../models/UselessTask';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-signalr',
  standalone: true,
  imports: [
    MatCardModule,
    MatFormFieldModule,
    FormsModule,
    CommonModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
  ],
  templateUrl: './signalr.component.html',
  styleUrls: ['./signalr.component.css'],
})
export class SignalrComponent implements OnInit {
  private hubConnection?: signalR.HubConnection;
  usercount = 0;
  tasks: UselessTask[] = [];
  taskname: string = '';

  ngOnInit(): void {
    console.log(this.tasks)
    this.connecttohub();

    console.log(this.tasks)

  }

  connecttohub() {
    // TODO On doit commencer par créer la connexion vers le Hub
    this.hubConnection = new signalR.HubConnectionBuilder()
                                    .withUrl("https://localhost:7289/hubController")
                                    .build();


    // TODO On peut commencer à écouter pour les évènements qui vont déclencher des callbacks
    // ajouter un .on qui écoute un message X et qui recoit la liste de tâches
    this.hubConnection.on('UserCount', (count) => {
      console.log(count)
      this.usercount = count;
    })

    this.hubConnection.on("V2", (tasks) => { 
      console.log(tasks);
      this.tasks = tasks;
// mettre la liste à jour
    });

    // TODO On doit ensuite se connecter
    // mettre le code de connexion
    this.hubConnection
        .start()
        .then(() => {
            console.log('La connexion est active!');
          })
        .catch(err => console.log('Error while starting connection: ' + err));
  }

  complete(id: number) {
    // TODO On invoke la méthode pour compléter une tâche sur le serveur
    this.hubConnection!.invoke('Complete', id);
    

  }

  addtask() {
    // TODO On invoke la méthode pour ajouter une tâche sur le serveur

    this.hubConnection?.invoke('AddTask', this.taskname);
    console.log("tâche ajoutée");
   
  }
}
