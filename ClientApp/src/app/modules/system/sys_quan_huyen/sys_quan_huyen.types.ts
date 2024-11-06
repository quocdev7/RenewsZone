export interface sys_quan_huyen_db {
    id: number;
    id_quoc_gia: number | null;
    id_tinh: number | null;
    ma: string;
    ten: string;
    note: string;
    update_by: string;
    create_by: string;
    create_date: string | null;
    update_date: string | null;
    status_del: number | null;
    ten_khong_dau: string;
}
export interface sys_quan_huyen_model {
    createby_name: string;
    updateby_name: string;
    quoc_gia: string;
    tinh: string;
    db: sys_quan_huyen_db;
}
