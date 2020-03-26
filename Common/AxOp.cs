﻿namespace Common
{
    public enum AxOp
    {
        None,
        [AxDisplay("اطلاعات پایه", "01", None)]
        Basic,
        [AxDisplay("مدیریت اشخاص", "01", Basic)]
        PeopleManagement,
        [AxDisplay("لیست اشخاص", "01", PeopleManagement)]
        PersonList,
        [AxDisplay("فرم شخص", "02", PeopleManagement)]
        PersonItem,
        [AxDisplay("ایجاد", "03", PeopleManagement)]
        PersonInsert,
        [AxDisplay("ویرایش", "04", PeopleManagement)]
        PersonUpdate,
        [AxDisplay("حذف شخص", "05", PeopleManagement)]
        PersonDelete,
        [AxDisplay("تصاویر", "01", PersonItem)]
        PersonImages,
        [AxDisplay("درج", "01", PersonImages)]
        PersonImageInsert,
        [AxDisplay("حذف", "02", PersonImages)]
        PersonImageDelete,
        [AxDisplay("مشاهده", "03", PersonImages)]
        PersonImageShow,
        [AxDisplay("مدیریت کاربران", "02", Basic)]
        UserManagement,
        [AxDisplay("لیست کاربران", "01", UserManagement)]
        UserList,
        [AxDisplay("فرم کاربر", "02", UserManagement)]
        UserItem,
        [AxDisplay("ایجاد", "03", UserManagement)]
        UserInsert,
        [AxDisplay("ویرایش", "04", UserManagement)]
        UserUpdate,
        [AxDisplay("حذف کاربر", "05", UserManagement)]
        UserDelete,
        [AxDisplay("لیست آدرس ها", "02", PersonItem)]
        AddressList,
        [AxDisplay("فرم آدرس", "03", PersonItem)]
        AddressItem,
        [AxDisplay("ایجاد", "04", PersonItem)]
        AddressInsert,
        [AxDisplay("ویرایش", "05", PersonItem)]
        AddressUpdate,
        [AxDisplay("حذف", "06", PersonItem)]
        AddressDelete,
        [AxDisplay("داشبورد", "03", Basic)]
        DashboardIndex,
        [AxDisplay("گزارشات", "99", None)]
        Reports,
        [AxDisplay("گزارشات اطلاعات پایه", "01", Reports)]
        BasicSystemReports,
        //[AxDisplay("گزارشات اشخاص", "01", BasicSystemReports)]
        //PersonReports,
        [AxDisplay("گزارش لیست اشخاص", "01", BasicSystemReports)]
        PersonListReports,
        [AxDisplay("مدیریت حقوق دسترسی", "04", Basic)]
        AuthorizationManagement,
        [AxDisplay("حقوق دسترسی عملیات ها", "01", AuthorizationManagement)]
        OperationsAuthorization,
        [AxDisplay("لیست گروه ها", "06", UserManagement)]
        GroupList,
        [AxDisplay("فرم گروه", "07", UserManagement)]
        GroupItem,
        [AxDisplay("ایجاد", "08", UserManagement)]
        GroupInsert,
        [AxDisplay("ویرایش", "09", UserManagement)]
        GroupUpdate,
        [AxDisplay("حذف گروه", "10", UserManagement)]
        GroupDelete,
    }
}