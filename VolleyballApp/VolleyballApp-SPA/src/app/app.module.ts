import { BrowserModule, HammerGestureConfig, HAMMER_GESTURE_CONFIG } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BsDropdownModule, BsDatepickerModule, PaginationModule, ButtonsModule, TabsModule, ModalModule } from 'ngx-bootstrap';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { appRoutes } from './routes';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { TeamListComponent } from './teams/team-list/team-list.component';
import { TeamCardComponent } from './teams/team-card/team-card.component';
import { TeamCreateComponent } from './teams/team-create/team-create.component';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { AuthGuard } from './_guards/auth.guard';
import { JwtModule } from '@auth0/angular-jwt';
import { TeamListResolver } from './_resolvers/team-list.resolver';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { InviteCardComponent } from './invites/invite-card/invite-card.component';
import { InviteListComponent } from './invites/invite-list/invite-list.component';
import { InviteListResolver } from './_resolvers/invite-list.resolver';
import { FriendListResolver } from './_resolvers/friend-list.resolver';
import { MemberFriendListComponent } from './members/member-friend-list/member-friend-list.component';
import { TimeAgoPipe } from 'time-ago-pipe';
import { NgPipesModule } from 'ngx-pipes';
import { TeamDetailComponent } from './teams/team-detail/team-detail.component';
import { TeamDetailResolver } from './_resolvers/team-detail.resolver';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { MatchListComponent } from './matches/match-list/match-list.component';
import { MatchListResolver } from './_resolvers/match-list.resolver';
import { MatchCardComponent } from './matches/match-card/match-card.component';
import { MatchDetailComponent } from './matches/match-detail/match-detail.component';
import { MatchDetailResolver } from './_resolvers/match-detail.resolver';
import { FileUploadModule } from 'ng2-file-upload';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import { TeamPhotoComponent } from './teams/team-photo/team-photo.component';



export function tokkenGetter() {
   return localStorage.getItem('token');
}

export class CustomHammerConfig extends HammerGestureConfig  {
   overrides = {
       pinch: { enable: false },
       rotate: { enable: false }
   };
}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MemberListComponent,
      MemberCardComponent,
      MemberEditComponent,
      MessagesComponent,
      TeamListComponent,
      TeamCardComponent,
      TeamCreateComponent,
      TeamDetailComponent,
      MemberDetailComponent,
      InviteCardComponent,
      InviteListComponent,
      MemberFriendListComponent,
      MemberMessagesComponent,
      TeamDetailComponent,
      MatchListComponent,
      MatchCardComponent,
      MatchDetailComponent,
      PhotoEditorComponent,
      TeamPhotoComponent,
      TimeAgoPipe
   ],
   imports: [
      BrowserModule,
      BrowserAnimationsModule,
      HttpClientModule,
      FormsModule,
      NgPipesModule,
      ReactiveFormsModule,
      SelectDropDownModule,
      FileUploadModule,
      BsDropdownModule.forRoot(),
      TabsModule.forRoot(),
      ModalModule.forRoot(),
      PaginationModule.forRoot(),
      ButtonsModule.forRoot(),
      BsDatepickerModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      JwtModule.forRoot({
         config: {
            tokenGetter: tokkenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/auth']
         }
      })
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      MessagesResolver,
      MemberListResolver,
      MemberEditResolver,
      InviteListResolver,
      TeamDetailResolver,
      TeamListResolver,
      MemberDetailResolver,
      FriendListResolver,
      MatchListResolver,
      MatchDetailResolver,
      AuthGuard,
      PreventUnsavedChanges,
      { provide: HAMMER_GESTURE_CONFIG, useClass: CustomHammerConfig }
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
