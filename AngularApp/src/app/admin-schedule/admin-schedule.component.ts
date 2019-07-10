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
  poruka :string;
  version :string;

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
      this.poruka = data; 
      if(this.poruka != "Please select line" &&  this.poruka != "Please select type" && this.poruka != "Input in wrong format" && this.poruka != "Schedule has changed by someone else. Please reload to get new version" && this.poruka != "Defined depature allredy excist" && this.poruka != "Changes Saved" && this.poruka != "Defined depature not excist")
      {
          this.Form.setValue({

              deps : data.Depatures,
          });
         
    } else
    {
      this.Form.setValue({

        deps : data,
    });
    }
    this.version = data.Version;
       });
  }

  Add()
  {
    
    this.service.AddDepature(this.type,this.line,this.AddForm.value.dep,this.version).subscribe((data)=> {
        this.mssg = data;
        this.service.GetDepatures(this.type,this.line).subscribe((data) => {
          this.poruka = data; 
          if(this.poruka != "Please select line" &&  this.poruka != "Please select type" && this.poruka != "Input in wrong format" && this.poruka != "Schedule has changed by someone else. Please reload to get new version" && this.poruka != "Defined depature allredy excist" && this.poruka != "Changes Saved" && this.poruka != "Defined depature not excist")
          {
              this.Form.setValue({
    
                  deps : data.Depatures,
              });
              this.version = data.Version;
        } else
        {
          this.Form.setValue({
    
            deps : data,
        });
        }
           });
    });
  }

  Remove()
  {
    this.service.RemoveDepature(this.type,this.line,this.RemoveForm.value.dep,this.version).subscribe((data)=> {
        this.mssg = data;
        this.service.GetDepatures(this.type,this.line).subscribe((data) => {
          this.poruka = data; 
          if(this.poruka != "Please select line" &&  this.poruka != "Please select type" && this.poruka != "Input in wrong format" && this.poruka != "Schedule has changed by someone else. Please reload to get new version" && this.poruka != "Defined depature allredy excist" && this.poruka != "Changes Saved" && this.poruka != "Defined depature not excist")
          {
              this.Form.setValue({
    
                  deps : data.Depatures,
              });
              this.version = data.Version;
        } else
        {
          this.Form.setValue({
    
            deps : data,
        });
        }
           });
    });
  }

}
