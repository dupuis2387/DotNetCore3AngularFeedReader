import { Component, OnInit, EventEmitter, Output , Input} from '@angular/core';


@Component({
  template: `<div class="embed-responsive embed-responsive-16by9">
      <div class="embed-responsive-item actAsDiv" id="player">
      </div>
    </div>`,
  selector: 'yt-video',
  styles: [`.max-width-1024 { max-width: 1024px; margin: 0 auto; }`],
})
export class VideoComponent implements OnInit {  

  
  @Input() videoid: string;

  public YT: any;
  public player: any;  
  
  constructor() { }

  ngOnInit() {

    this.YT = window['YT'];
      
    this.player = new window['YT'].Player('player', {
        videoId: this.videoid,
        
    });

  }



  
}
