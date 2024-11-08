import { IsActiveMatchOptions } from '@angular/router';

export interface FuseNavigationItem
{
    id?: string;
    title?: string;
    subtitle?: string;
    module?:string;
    type:
        | 'aside'
        | 'basic'
        | 'collapsable'
        | 'divider'
        | 'group'
        | 'spacer';
    hidden?: (item: FuseNavigationItem) => boolean;
    active?: boolean;
    translate: string;
    disabled?: boolean;
    link?: string;
    externalLink?: boolean;
    exactMatch?: boolean;
    isActiveMatchOptions?: IsActiveMatchOptions;
    function?: (item: FuseNavigationItem) => void;
    classes?: {
        title?: string;
        subtitle?: string;
        icon?: string;
        wrapper?: string;
    };
    icon?: string;
    icon_img?: string;
    badge?: {
        title?: string;
        classes?: string;
    };
    children?: FuseNavigationItem[];
    meta?: any;
}

export type FuseVerticalNavigationAppearance =
    | 'default'
    | 'compact'
    | 'dense'
    | 'thin';

export type FuseVerticalNavigationMode =
    | 'over'
    | 'side';

export type FuseVerticalNavigationPosition =
    | 'left'
    | 'right';
