import { HttpClient } from '@angular/common/http';

import { MatDialog } from '@angular/material/dialog';
import { Component, ViewChild, Inject, Directive, OnDestroy, QueryList, ViewChildren } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';


import { ActivatedRoute } from '@angular/router';
import { FuseNavigationService } from '@fuse/components/navigation';
import { TranslocoService } from '@ngneat/transloco';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { doc_tailieu_view_file_onlineComponent } from 'app/modules/system/sys_user/viewfileonline.component';
import { clear } from 'console';
import { translateDataTable } from '@fuse/components/commonComponent/VietNameseDataTable';



@Directive()
export abstract class BaseIndexDatatableComponent implements OnDestroy {

    public action: string;
    public controller: String;
    public filter: any;
    public table: any;
    @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
    public pageLoading: Boolean = false;
    public dtOptions: any = {};
    public currentIndex: number;
    public listData: any = [];
    public baseurl: String;
    constructor(public http: HttpClient,
        _baseUrl: string,
        public _translocoService: TranslocoService,
        public _fuseNavigationService: FuseNavigationService,
        public route: ActivatedRoute,
        public dialog: MatDialog, _controller: String, _filter: any) {
        this.controller = _controller;
        this.baseurl = _baseUrl;
        this.filter = _filter;
        this.pageLoading = false;
        $(document).on('click', '.mat-focus-indicator.mat-icon-button.mat-button-base, .mat-tab-label',
            () => setTimeout(() => {
                if (this.dtElements.length > 0) {
                    this.dtElements.forEach((dtElement: DataTableDirective) => {
                        dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
                            dtInstance.columns.adjust();
                        });
                    });
                }
            }
                , 500)
        )
    }
    ngOnDestroy(): void {

    }
    public rerender(): void {

        this.dtElements.forEach((dtElement: DataTableDirective) => {
            dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.ajax.reload(null, true);
            });
        });
        // Destroy the table first
    };


    public getFontAwesomeIconFromMIME(mimeType) {
        // List of official MIME Types: http://www.iana.org/assignments/media-types/media-types.xhtml
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

        for (var key in icon_classes) {
            if (icon_classes.hasOwnProperty(key)) {
                if (mimeType.search(key) === 0) {
                    // Found it
                    return icon_classes[key];
                }
            } else {
                return "assets/icon_file_type/file.png";
            }
        }
    }
    public formatSizeUnits(bytes) {
        if (bytes >= 1073741824) { bytes = (bytes / 1073741824).toFixed(2) + " GB"; }
        else if (bytes >= 1048576) { bytes = (bytes / 1048576).toFixed(2) + " MB"; }
        else if (bytes >= 1024) { bytes = (bytes / 1024).toFixed(2) + " KB"; }
        else if (bytes > 1) { bytes = bytes + " bytes"; }
        else if (bytes == 1) { bytes = bytes + " byte"; }
        else { bytes = "0 bytes"; }
        return bytes;
    }
    openDialogViewFileOnline(url_download, file_name, file_type, file_size, url_view_online): void {
        var dialogRef = this.dialog.open(
            doc_tailieu_view_file_onlineComponent,
            {
                disableClose: true,
                panelClass: ['full-screen-modal'],
                data: {
                    url: url_view_online,
                    url_download: url_download,
                    file_name: file_name,
                    file_type: file_type,
                    file_size: file_size,
                },
            }
        );
        dialogRef.afterClosed().subscribe((result) => { });
    }
    public rerenderfilter(): void {
        this.before_filter();
        this.dtElements.forEach((dtElement: DataTableDirective) => {
            dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.ajax.reload(null, true);
            });
        });
    }
    public before_filter(): void {

    }
    public showMessagewarning(msg): void {
        Swal.fire({
            title: msg,
            text: "",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: this._translocoService.translate('close'),
        }).then((result) => {

        })

    }
    public showMessagewarning2(title, msg): void {
        Swal.fire({
            title: title,
            text: msg,
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: this._translocoService.translate('close'),
        }).then((result) => {

        })

    }


    public showMessageSuccess(msg): void {
        Swal.fire({
            title: msg,
            text: "",
            icon: 'success',
            confirmButtonColor: '#3085d6',
            confirmButtonText: this._translocoService.translate('close'),
        }).then((result) => {

        })

    }
    public delete(id1): void {
        Swal.fire({
            title: this._translocoService.translate('areYouSure'),
            text: "",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: this._translocoService.translate('yes'),
            cancelButtonText: this._translocoService.translate('no')
        }).then((result) => {
            if (result.value) {
                this.http
                    .post(this.controller + '.ctr/delete/',
                        {
                            id: id1,
                        }
                    ).subscribe(resp => {
                        Swal.fire('Ngưng sử dụng thành công', '', 'success');
                        this.rerender();
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

    }
    revertStatus(model, pos): void {
        model.db.status_del = 1;
        this.http
            .post(this.controller + '.ctr/edit/', { data: model, }).subscribe(resp => {
                Swal.fire('Sử dụng thành công', '', 'success');
                this.rerender();
            },
                error => {
                    if (error.status == 403) {
                        Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                    }
                });
        this.rerender();
    }
    public close(id1): void {
        Swal.fire({
            title: this._translocoService.translate('areYouSure'),
            text: "",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: this._translocoService.translate('yes'),
            cancelButtonText: this._translocoService.translate('no')
        }).then((result) => {
            if (result.value) {
                this.http
                    .post(this.controller + '.ctr/close/',
                        {
                            id: id1,
                        }
                    ).subscribe(resp => {
                        this.rerender();
                    });
            }
        })

    }

    public baseInitData(): void {
        this.handleDataBefore();
        const that = this;
        this.dtOptions = {
            language: translateDataTable,
            scrollY: '50vh',
            scrollCollapse: true,
            retrieve: true,
            scrollX: true,
            fixedHeader: true,
            ordering: false,
            serverSide: true,
            processing: true,
            lengthMenu: [50, 75, 100],
            "drawCallback": function (settings) {
                var api = this.api();
                that.table = api;
                setTimeout(function () { api.columns.adjust(); }, 300);
                $('tbody').on('click', 'tr', function () {
                    if ($(this).hasClass('selected')) {
                        $(this).removeClass('selected');
                    }
                    else {
                        $('tr.selected').removeClass('selected');
                        $(this).addClass('selected');
                    }
                });

            },
            responsive: {
                details: {
                    renderer: function (api, rowIdx, columns) {
                        setTimeout(function () {
                            api.columns.adjust();
                        }, 300);
                    }
                }
            },
            "searching": false,
            ajax: (data, callback, settings) => {
                this.pageLoading = true;
                this.http
                    .post<DataTablesResponse>(this.baseurl + '' + this.controller + '.ctr/DataHandler/',
                        {
                            param1: data,
                            data: this.filter
                        }
                    ).subscribe(resp => {
                        that.listData = resp.data;
                        this.pageLoading = false;
                        that.currentIndex = resp.start;
                        callback({
                            recordsTotal: resp.recordsTotal,
                            recordsFiltered: resp.recordsFiltered,
                            data: []
                        });
                    });
            },

        };


    }

    public handleDataBefore(): void {


    }




}
