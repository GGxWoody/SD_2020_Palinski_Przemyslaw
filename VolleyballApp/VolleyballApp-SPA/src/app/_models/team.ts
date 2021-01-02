import { League } from './league';
import { Photo } from './photo';
import { User } from './user';
import { UserTeam } from './userTeam';

export interface Team {
    id: number;
    teamName: string;
    owner?: User;
    users?: User[];
    userTeams?: UserTeam[];
    dateCreated?: Date;
    description?: string;
    photo?: Photo;
    photoUrl?: string;
    league?: League;
    rankingPoints?: number;
}
