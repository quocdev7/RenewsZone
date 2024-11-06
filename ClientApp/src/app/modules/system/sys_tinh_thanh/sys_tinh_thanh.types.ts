export interface sys_tinh_thanh_db {
    id: number;
    id_quoc_gia: number | null;
    ma: string;
    ten: string;
    update_by: string;
    create_by: string;
    create_date: string | null;
    update_date: string | null;
    status_del: number | null;
    note: string;
    ten_khong_dau: string;
}
export interface sys_tinh_thanh_model {
    createby_name: string;
    updateby_name: string;
    quoc_gia: string;
    db: sys_tinh_thanh_db;
}