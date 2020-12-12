import { Match } from './match';
import { Team } from './team';
import { User } from './user';

export interface Invite {
    id: number;
    inviteFrom?: User;
    inviteTo?: User;
    teamInviting?: Team;
    teamInvited?: Team;
    matchInvitedTo: Match;
    friendInvite: boolean;
    teamInvite: boolean;
    matchInvite: boolean;
    refereeInvite: boolean;
}
