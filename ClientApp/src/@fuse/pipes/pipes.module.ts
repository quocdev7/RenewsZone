import { NgModule } from '@angular/core';
import { CamelCaseToDashPipe } from './camelCaseToDash.pipe';

import { filterequalPipe, FilterPipe } from './find-by-key/filter/filter.pipe';
import { FileTypePipe } from './formatFileType.pipe';
import { GetByIdPipe } from './getById.pipe';
import { HtmlToPlaintextPipe } from './htmlToPlaintext.pipe';
import { KeysPipe } from './keys.pipe';
import { safeHtml, SafePipe } from './safe.pipe';
import { StrLimitPipe } from './strLimit.pipe';
import { ThousandSuffixesPipe } from './ThousandSuffixes.pipe';


@NgModule({
    declarations: [
        KeysPipe,
        GetByIdPipe,
        HtmlToPlaintextPipe,
        SafePipe,
        safeHtml,
        CamelCaseToDashPipe,
        FilterPipe,
        filterequalPipe,
        ThousandSuffixesPipe,
        FileTypePipe,
        StrLimitPipe,
    ],
    providers:    [ ThousandSuffixesPipe  ],
    imports     : [],
    exports     : [
        KeysPipe,
        GetByIdPipe,
        HtmlToPlaintextPipe,
        ThousandSuffixesPipe,
        SafePipe,
        safeHtml,
        CamelCaseToDashPipe,
        FilterPipe,
        filterequalPipe,
        FileTypePipe,
        StrLimitPipe
    ]
})
export class FusePipesModule
{
}
