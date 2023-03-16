import { Component, OnInit} from '@angular/core';
import { Todo } from 'src/app/models/Todo';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { TodoService } from 'src/app/services/todo.services';
import { FormControl, FormGroup } from '@angular/forms';
import { UntypedFormGroup, UntypedFormBuilder } from "@angular/forms";
import { take } from "rxjs/operators";

@Component({
  selector: 'app-todos',
  templateUrl: './todos.component.html',
  styleUrls: ['./todos.component.css']
})

export class TodosComponent implements OnInit {

  formulario: any;
  formularioPesquisa: UntypedFormGroup;
  tituloFormulario: string;
  todos: Todo[] = [];
  allTodos: Todo[] = [];
  Id: number;
  Descricao: string;
  Status: string;

  enableTable: boolean = true;
  enableForm: boolean = false;
  loading = false;

  modalRef: BsModalRef;

  constructor(private todoService: TodoService,
              private modalService: BsModalService,
              private formBuilder: UntypedFormBuilder
             ) {}

    ngOnInit(): void {
      this.formularioPesquisa = this.formBuilder.group({
        Descricao: [null],
    });
      this.todoService.FindAllTodo().subscribe((result) => {
        this.todos = result;
      });
    }

    ViewFormPost(): void {
      this.enableTable = false;
      this.enableForm = true;
      this.tituloFormulario = 'Cadastrar nova Tarefa';
      this.formulario = new FormGroup({
        descricao: new FormControl(null),
        data: new FormControl(null),
        status: new FormControl(null)
      });
    }

    SendFormOfPost(): void {
      const todo: Todo = this.formulario.value;

      this.todoService.PostTodo(todo).subscribe((result) => {
        this.enableForm = false;
        this.enableTable = true;
        alert('Tarefa cadastrado com sucesso');
        this.todoService.FindAllTodo().subscribe((registers) => {
          this.todos = registers;
          });
        });
    }

    SearchTodoByDescription():void {
      this.loading = true;

      if (this.formularioPesquisa.value.descricao) {
          this.todoService
              .FindByDescription(this.Descricao)
              .pipe(take(1))
              .subscribe((result) => {
                   this.todos.filter(x => x.Descricao === result.Descricao);
              });
        }
  }
  //   SearchTodoByDescription(e: Event): void {
  //     const target = e.target as HTMLInputElement;
  //     const value = target.value;

  //     this.todos = this.allTodos.filter((x) =>
  //       x.Descricao.toUpperCase().includes(value)
  //     );
  //     this.todoService.FindByDescription(value).subscribe((result) => {
  //       this.Descricao = result.Descricao
  //     });
  //  }

  CleanSearch(): void{

     this.enableTable = true;
     this.enableForm = false;
     this.todoService.FindAllTodo().subscribe((result) => {
       this.todos = result;
      });
   }

    Voltar(): void {
      this.enableTable = true;
      this.enableForm = false;
      this.todoService.FindAllTodo().subscribe((result) => {
        this.todos = result;
      });
    }
}

