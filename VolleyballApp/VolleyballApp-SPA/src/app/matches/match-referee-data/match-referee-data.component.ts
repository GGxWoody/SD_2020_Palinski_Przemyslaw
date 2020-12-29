import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Match } from 'src/app/_models/match';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FriendInviteService } from 'src/app/_services/friend-invite.service';

@Component({
  selector: 'app-match-referee-data',
  templateUrl: './match-referee-data.component.html',
  styleUrls: ['./match-referee-data.component.scss']
})
export class MatchRefereeDataComponent implements OnInit {
  @Input() referee: User;
  @Input() match: Match;
  @Output() matchChange: EventEmitter<Match> = new EventEmitter<Match>();
  user: User = JSON.parse(localStorage.getItem('user'));
  constructor(private inviteService: FriendInviteService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  sendMatchInvite() {
    this.inviteService.sendRefereeInvite(this.user.id, this.referee.id, this.match.id).subscribe(data => {
      this.alertify.success('You invited: ' + this.referee.knownAs + ' to be referee');
      this.matchChange.emit(this.match);
    }, error => {
      this.alertify.error(error);
    });
  }

}
