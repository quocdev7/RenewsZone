import { Component, Inject, HostListener } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';

import { HttpClient, HttpEventType, HttpResponse } from '@angular/common/http';

import { TranslocoService } from '@ngneat/transloco';

import { ActivatedRoute } from '@angular/router';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import Swal from 'sweetalert2';
import { DomSanitizer } from '@angular/platform-browser';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';

@Component({
    selector: 'doc_tailieu_view_file_online',
    templateUrl: 'viewfileonline.component.html'
})
export class doc_tailieu_view_file_onlineComponent  {

    public loading = false;
    public srcfile: any = "";
    public record: any;
    constructor(public dialogRef: MatDialogRef<doc_tailieu_view_file_onlineComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        private sanitizer: DomSanitizer,
         public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        this.record = data;

        var icon_classes = {
            // Media
            "image/jpeg": "assets/icon_file_type/jpg.png",
            "image/png": "assets/icon_file_type/png.png",
            // Documents
            "application/pdf": "assets/icon_file_type/pdf.png",
            "application/msword": "assets/icon_file_type/doc.png",
            "application/vnd.ms-word": "assets/icon_file_type/doc.png",
            "application/vnd.oasis.opendocument.text": "assets/icon_file_type/doc.png",
            "application/vnd.openxmlformats-officedocument.wordprocessingml":
                "assets/icon_file_type/doc.png",
            "application/vnd.ms-excel": 'assets/icon_file_type/excel.png',
            "application/vnd.openxmlformats-officedocument.spreadsheetml":
                'assets/icon_file_type/excel.png',
            "application/vnd.oasis.opendocument.spreadsheet": "assets/icon_file_type/excel.png",
            "application/vnd.ms-powerpoint": "assets/icon_file_type/ppt.png",
            "application/vnd.openxmlformats-officedocument.presentationml":
                "assets/icon_file_type/ppt.png",
            "application/vnd.oasis.opendocument.presentation": "assets/icon_file_type/ppt.png",
            "text/plain": "assets/icon_file_type/txt.png",
            "text/html": "assets/icon_file_type/html.png",
            "application/json": "assets/icon_file_type/json-file.png",
            // Archives
            "application/gzip": "assets/icon_file_type/zip.png",
            "application/x-zip-compressed": "assets/icon_file_type/zip.png",
            "application/octet-stream": "assets/icon_file_type/zip-1.png"
        };
        if (data.file_type == "text/plain" ||
            data.file_type == "text/html" ||
            data.file_type == "application/json" ||
            data.file_type == "application/pdf" ||
            data.file_type == "image/jpeg" ||
            data.file_type == "image/png"
        ) {
            var url = data.url.replace("/download?id=","/viewonline?id=");
            this.srcfile = "https://" + window.location.host + "" + url;
        } else {
            this.srcfile = 'https://view.officeapps.live.com/op/view.aspx?src=' + encodeURI("https://" + window.location.host + "" + data.url);
        }
     
        this.innerHeight = window.innerHeight;
        //  this.loadInfo();
       
    }
    public innerHeight: any;
    public formatSizeUnits(bytes) {
        if (bytes >= 1073741824) { bytes = (bytes / 1073741824).toFixed(2) + " GB"; }
        else if (bytes >= 1048576) { bytes = (bytes / 1048576).toFixed(2) + " MB"; }
        else if (bytes >= 1024) { bytes = (bytes / 1024).toFixed(2) + " KB"; }
        else if (bytes > 1) { bytes = bytes + " bytes"; }
        else if (bytes == 1) { bytes = bytes + " byte"; }
        else { bytes = "0 bytes"; }
        return bytes;
    }
    @HostListener('window:resize', ['$event'])
    onResize(event) {
        this.innerHeight = window.innerHeight;
    }
    close(): void {
        this.dialogRef.close(this.record);
    }

    trustfile(url) {
        this.sanitizer.bypassSecurityTrustResourceUrl(url);
    }

  
   
   
  

}

