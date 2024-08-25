import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface Task {
  description: string;
}

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  httpClient = inject(HttpClient);

  getTasks(): Observable<Task[]> {
    return this.httpClient.get<Task[]>('http://localhost:5250/tasks');
  }

  addTask(task: Task): Observable<Task> {
    return this.httpClient.post<Task>('http://localhost:5250/tasks', task);
  }
}
