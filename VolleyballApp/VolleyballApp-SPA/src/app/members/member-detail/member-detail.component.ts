import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { TabsetComponent } from 'ngx-bootstrap';
import { AuthService } from 'src/app/_services/auth.service';
import { FriendInviteService } from 'src/app/_services/friend-invite.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.scss']
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
  user: User;

  constructor(private authService: AuthService, private userService: UserService,
              private alertify: AlertifyService, private root: ActivatedRoute,
              private inviteService: FriendInviteService) { }

  ngOnInit() {
    this.root.data.subscribe(data => {
      this.user = data.user;
    });
  }

  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }

  sendInvite(id: number) {
    this.inviteService.sendInvite(this.authService.decodedToken.nameid, id).subscribe(data => {
      this.alertify.success('You invited: ' + this.user.knownAs + ' to friendlist');
    }, error => {
      this.alertify.error(error);
    });
  }
}
