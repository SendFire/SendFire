import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import Typed from 'typed.js';
import { JobService } from '../../services/job.service';

function scrollToBottom(id) {
  const div = document.getElementById(id);
  if(!div) { return; }
  div.scrollTop = div.scrollHeight - div.clientHeight;
}

@Component({
  selector: 'sf-terminal',
  templateUrl: './terminal.component.html',
  styleUrls: ['./terminal.component.scss']
})
export class TerminalComponent implements OnInit {
  private _messages:string[] = [];
  private messageList:string[] = [];
  private _id:string;
  private _scrollInterval:any;
  @Input() 
  set id(passedId) {
    this._id = passedId;
    this.fetchResult();
  };

  constructor(private _jobService: JobService){}

  ngOnInit() {
  }

  startInterval(){

    this._scrollInterval = setInterval(() => {
       var elem = document.getElementById('main-term');
       if(!elem) {return;}
       elem.scrollTop = elem.scrollHeight;
      }, 500);

  }

  addMessage(msg) {
    this._messages.push(msg);
    this.messageList.push(`msg${this._messages.length}`);
  }

  updateTerminal() {
    var output = this._messages[this._messages.length - 1];
    setTimeout(() => {
      var d = document.getElementById(`msg${this._messages.length}`);
      if(!d) {
        return;
      }
      const typd = new Typed(`#msg${this._messages.length}`, {
        strings: [output],
        typeSpeed: 1,
        backSpeed: 0,
        loop: false,
        showCursor: false,
        cursorChar: '_',
        onComplete: (self) => {
          setTimeout(() => {
            clearInterval(this._scrollInterval);
          }, 900);
          
        },
      });

      this.startInterval();
    }, 500);

    
  }

  fetchResult() {
    console.log("calling")
    this._jobService.getResults(this._id).subscribe(results => {
        if(!results || !results.results || !results.results.Result) {
          this.addMessage('Processing');
          this.updateTerminal();
          setTimeout(() => { console.log("setting call"); this.fetchResult() }, 5000);
          return;
        }
        const result = results.results.Result.substr(1).slice(0, -1).replace(/\\\\/g, "\\").replace(/</g,"&lt;").replace(/>/g,"&gt;").replace(/(?:\\[rn])+/g, "<br />");
        this.addMessage(result);
        this.updateTerminal();

    }, () => {
       
    })  
  }
  title = 'app';
}
