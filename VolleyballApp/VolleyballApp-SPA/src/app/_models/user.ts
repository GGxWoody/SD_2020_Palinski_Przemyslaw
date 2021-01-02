import { PhotoEditorComponent } from '../members/photo-editor/photo-editor.component';
import { Photo } from './photo';
import { Team } from './team';
import { UserTeam } from './userTeam';

export interface User {
    id: number;
    username: string;
    userType?: string;
    knownAs?: string;
    age?: number;
    gender?: string;
    created?: Date;
    photoUrl?: string;
    city?: string;
    description?: string;
    positions?: string;
    mail?: string;
    country?: string;
    lastActive?: Date;
    photo?: Photo;
    intrests?: string;
    introduction?: string;
    lookingFor?: string;
    userTeam?: UserTeam;
    gamesWon?: number;
    gamesLost?: number;
    isFriend?: boolean;
    gamesPlayed?: number;
    isMailActivated?: boolean;
    rankingPoints?: number;
}
