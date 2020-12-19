import { TeamLeague } from './teamLeague';
import { User } from './user';

export class League {
    id: number;
    teamLimit: number;
    country: string;
    city: string;
    description: string;
    closedSignUp: Date;
    creator: User;
    teamLeague: TeamLeague[];
}
