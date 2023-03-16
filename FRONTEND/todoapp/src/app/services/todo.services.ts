import  {Injectable} from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Todo } from "../models/Todo";

const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

@Injectable({
    providedIn: 'root'
})

export class TodoService{
    url = 'https://localhost:5000/todos';

    constructor(private http: HttpClient) {}

    FindAllTodo(): Observable<Todo[]> {
        const apiUrl = `${this.url}/listaTodos`
        return this.http.get<Todo[]>(apiUrl);
      };

    FindByDescription(descricao: string): Observable<Todo> {
       const apiUrl = `${this.url}/${descricao}`;
       return this.http.get<Todo>(apiUrl);
    };

    PostTodo(todo: Todo): Observable<Todo> {
      const apiUrl = `${this.url}/post`;
      return this.http.post<Todo>(apiUrl, todo, httpOptions);
    };
}
