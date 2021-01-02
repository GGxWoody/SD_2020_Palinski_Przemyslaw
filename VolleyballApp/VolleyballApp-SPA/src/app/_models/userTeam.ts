import { Team } from './team';
import { User } from './user';

export class UserTeam {
    userId: number;
    user: User;
    teamId: number;
    team: Team;
    isTeamOwner: boolean;
}
