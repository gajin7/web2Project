import { Component, OnInit } from '@angular/core';
import { AdminScheduleService } from './admin-schedule.service';
import { Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-admin-schedule',
  templateUrl: './admin-schedule.component.html',
  styleUrls: ['./admin-schedule.component.css']
})
export class AdminScheduleComponent implements OnInit {

  lines : string[];
  line: string;
  type: string;
  mssg : string;

  Form = this.fb.group({
    deps: ['',Validators.required],
  });


  AddForm = this.fb.group({
    dep: ['',Validators.required],
  });

  RemoveForm = this.fb.group({
    dep: ['',Validators.required],
  });
  

  constructor(public service: AdminScheduleService, public fb : FormBuilder) { }

  ngOnInit() {
    this.service.GetLines().subscribe((data) => { 
      this.lines = data; });
  }

  selectedLine (event: any) {
    //update the ui
    this.line = event.target.value;
  }

  selectedType (event: any) {
    //update the ui
    this.type = event.target.value;
    
  }

  Show()
  {
    this.service.GetDepatures(this.type,this.line).subscribe((data) => { 
      this.Form.setValue({
        deps : data,
      });
       });
  }

  Add()
  {
    this.service.AddDepature(this.type,this.line,this.AddForm.value.dep).subscribe((data)=> {
        this.mssg = data;
        this.service.GetDepatures(this.type,this.line).subscribe((data) => { 
          this.Form.setValue({
            deps : data,});
          });
    });
  }

  Remove()
  {
    this.service.RemoveDepature(this.type,this.line,this.RemoveForm.value.dep).subscribe((data)=> {
        this.mssg = data;
        this.service.GetDepatures(this.type,this.line).subscribe((data) => { 
          this.Form.setValue({
            deps : data,});
          });
    });
  }

}
