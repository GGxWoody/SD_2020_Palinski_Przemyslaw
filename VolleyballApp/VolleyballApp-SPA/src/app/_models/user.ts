import { Team } from './team';

export interface User {
    id: number;
    username: string;
    knownAs: string;
    age: number;
    gender: string;
    created: Date;
    photoUrl: string;
    city: string;
    country: string;
    lastActive: Date;
    intrests?: string;
    introduction?: string;
    lookingFor?: string;
    teams?: Team[];
    teamsCreated?: Team[];
    gamesWon?: number;
    gamesLost?: number;
}
