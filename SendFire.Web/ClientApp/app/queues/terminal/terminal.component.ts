import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import Typed from 'typed.js';
import { JobService } from '../../services/job.service';



@Component({
  selector: 'sf-terminal',
  templateUrl: './terminal.component.html',
  styleUrls: ['./terminal.component.scss']
})
export class TerminalComponent implements OnInit {
  private typeOut:boolean = false;
  private _output: string[] = [];
  private _messages: string[] = [];
  private messageList: string[] = [];
  private _id: string;
  private _scrollInterval: any;
  @Input()
  set id(passedId) {
    this._id = passedId;
    if(!this._id) {return;}
    this.fetchResult();
  };

  constructor(private _jobService: JobService) { }

  ngOnInit() {

  }
  
  scrollToBottom() {
    var elem = document.getElementById('main-term');
    if (!elem) { return; }
    elem.scrollTop = elem.scrollHeight;
  }
  startInterval() {

    this._scrollInterval = setInterval(() => {
      this.scrollToBottom();
    }, 45);

  }

  addProcessing() {
    let processing = this._messages[0];
    if (!processing) {
      this.addMessage("Running...");
    }
  }

  addMessage(msg) {
    this._messages.push(msg);
    this.messageList.push(`msg${this._messages.length}`);
  }

  updateTerminal() {
    var output = this._messages[this._messages.length - 1];
    var split = output.split("<br />");
    if(this.typeOut) {
      this.updateTerminalTyped(output);
    } else {
      this.updateTerminalNoTyped(split);
    }


  }

  updateTerminalTyped(output) {
    this.startInterval()
    setTimeout(() => {
      var d = document.getElementById(`msg${this._messages.length}`);
      if (!d) {
        return;
      }

      const typd = new Typed(`#msg${this._messages.length}`, {
        strings: [output],
        typeSpeed: 0,
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
    }, 500);
  }

  updateTerminalNoTyped(split) {
    
    this.startInterval();
    split.forEach((s, i) => {
      setTimeout(() => {
        this._output.push(s)
        if (i === split.length - 1) {
          setTimeout(() => {
            clearInterval(this._scrollInterval);
          }, 200)
        }
      }, 50 * i);
    })
  }

  fetchResult() {
    
    this._jobService.getResults(this._id).subscribe(results => {
      if (!results || !results.results || !results.results.Result) {
        this.addProcessing();
        this.updateTerminal();
        setTimeout(() => { this.fetchResult() }, 5000);
        return;
      }
      const result = results.results.Result.substr(1).slice(0, -1)
        .replace(/</g, "&lt;").replace(/>/g, "&gt;")
        .replace(/(?:\\[rn])+/g, "<br />")
        .replace(/\\\\/g, "\\");
      this.addMessage(result);
      this.updateTerminal();

    }, () => {

    })
  }
  title = 'app';
}
