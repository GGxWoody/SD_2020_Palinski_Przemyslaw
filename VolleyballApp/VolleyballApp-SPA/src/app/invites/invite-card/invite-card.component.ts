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


  acceptInvite(userId: number, id: number) {
    this.inviteService.acceptInvite(userId, id).subscribe(data => {
      this.alertify.success('You accepted: ' + this.invite.inviteFrom.knownAs + ' to friendlist');
      this.refreshContent.emit();
    }, error => {
      this.alertify.error(error);
    });
  }

  declineInvite(userId: number, id: number) {
    this.inviteService.declineInvite(userId, id).subscribe(data => {
      this.alertify.success('You declined ' + this.invite.inviteFrom.knownAs + ' invite to friendlist');
      this.refreshContent.emit();
    }, error => {
      this.alertify.error(error);
    });
  }
}
