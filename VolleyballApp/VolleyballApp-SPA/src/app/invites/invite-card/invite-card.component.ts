import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Invite } from 'src/app/_models/invite';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FriendInviteService } from 'src/app/_services/friend-invite.service';

@Component({
  selector: 'app-invite-card',
  templateUrl: './invite-card.component.html',
  styleUrls: ['./invite-card.component.scss']
})
export class InviteCardComponent implements OnInit {
  @Input() invite: Invite;
  @Output() refreshContent = new EventEmitter();
  constructor(private inviteService: FriendInviteService, private alertify: AlertifyService) { }

  ngOnInit() {
  }


  acceptFriendInvite(userId: number, id: number) {
    this.inviteService.acceptFriendInvite(userId, id).subscribe(data => {
      this.alertify.success('You accepted: ' + this.invite.inviteFrom.knownAs + ' to friendlist');
      this.refreshContent.emit();
    }, error => {
      this.alertify.error(error);
    });
  }

  declineFriendInvite(userId: number, id: number) {
    this.inviteService.declineFriendInvite(userId, id).subscribe(data => {
      this.alertify.success('You declined ' + this.invite.inviteFrom.knownAs + ' invite to friendlist');
      this.refreshContent.emit();
    }, error => {
      this.alertify.error(error);
    });
  }

  acceptTeamInvite(userId: number, teamId: number, id: number) {
    this.inviteService.acceptTeamInvite(userId, teamId, id).subscribe(data => {
      this.alertify.success('You accepted: ' + this.invite.teamInvited.owner.knownAs
      + ' invite to team ' + this.invite.teamInvited.teamName);
      this.refreshContent.emit();
    }, error => {
      this.alertify.error(error);
    });
  }

  declineTeamInvite(userId: number, teamId: number, id: number) {
    this.inviteService.declineTeamInvite(userId, teamId, id).subscribe(data => {
      this.alertify.success('You declined ' + this.invite.teamInvited.owner.knownAs
      + ' invite to team ' + this.invite.teamInvited.teamName);
      this.refreshContent.emit();
    }, error => {
      this.alertify.error(error);
    });
  }
}
