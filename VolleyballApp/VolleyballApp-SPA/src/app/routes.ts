import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { InviteListComponent } from './invites/invite-list/invite-list.component';
import { MatchDetailComponent } from './matches/match-detail/match-detail.component';
import { MatchListComponent } from './matches/match-list/match-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberFriendListComponent } from './members/member-friend-list/member-friend-list.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { TeamDetailComponent } from './teams/team-detail/team-detail.component';
import { TeamListComponent } from './teams/team-list/team-list.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { FriendListResolver } from './_resolvers/friend-list.resolver';
import { InviteListResolver } from './_resolvers/invite-list.resolver';
import { MatchDetailResolver } from './_resolvers/match-detail.resolver';
import { MatchListResolver } from './_resolvers/match-list.resolver';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { TeamDetailResolver } from './_resolvers/team-detail.resolver';
import { TeamListResolver } from './_resolvers/team-list.resolver';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            {
                path: 'members/edit', component: MemberEditComponent, resolve: { user: MemberEditResolver },
                canDeactivate: [PreventUnsavedChanges]
            },
            { path: 'members', component: MemberListComponent, resolve: { users: MemberListResolver } },
            { path: 'friends', component: MemberFriendListComponent, resolve: { friends: FriendListResolver } },
            { path: 'teams', component: TeamListComponent, resolve: {teams: TeamListResolver} },
            { path: 'matches', component: MatchListComponent, resolve: {matches: MatchListResolver} },
            { path: 'invites', component: InviteListComponent, resolve: {invites: InviteListResolver} },
            { path: 'messages', component: MessagesComponent, resolve: {messages: MessagesResolver} },
            { path: 'members/:id', component: MemberDetailComponent,
             resolve: { user: MemberDetailResolver, loginedInUser: MemberEditResolver } },
            { path: 'teams/:id', component: TeamDetailComponent,
            resolve: { team: TeamDetailResolver, loginedInUser: MemberEditResolver} },
            { path: 'matches/:id', component: MatchDetailComponent, resolve: { match: MatchDetailResolver } }
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' },
];
