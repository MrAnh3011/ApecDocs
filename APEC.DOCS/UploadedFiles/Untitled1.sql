SELECT * FROM (
    SELECT distinct a.*,
        s.tax_number, s.is_marriage, s.folk, s.religion,s.nationality,
        s.country country1,s.resident,s.private_email,
        s.edu_level, s.height, s.heavy, s.bank_account, s.bank_name,
        s.car_registered, s.car_type, s.car_id, s.car_color, s.note note_d,       
        s.insurance_number, s.hot_name, s.hot_phone, s.hot_relate,
fnc_get_cmd_separated_hrc(a.staff_id) certificate,
fnc_get_cmd_separated_hrp(a.staff_id) process,
fnc_get_cmd_separated_hrr(a.staff_id) relate
from apec_staff a 
left JOIN apec_hr_certificate c on a.staff_id = c.staff_id
left join apec_hr_process p on a.staff_id = p.staff_id
left join apec_hr_relate r on a.staff_id = r.staff_id
left join apec_hr_staff_detail s on a.staff_id = s.staff_id
WHERE A.staff_ID = 'API190819001'
);

--p.from_date, p.to_date,  p.position_type, p.salary, p.note note4,

select 
'Ngày BÐ: ' || to_char(from_date, 'dd/MM/yyyy') || 
' - Ngày KT: ' || to_char(to_date, 'dd/MM/yyyy') || 
' - V? trí: ' || position_type  || CERTIFICATE
' - Luong: ' || salary ||
' - Ghi chú: ' || note 
from apec_hr_process ;

SELECT * from apec_hr_certificate c;

select * from deflang a where a.langNAME like '%SEARCH_STAFF_FULL$%';

select * from apec_hr_staff_detail a where a.staff_ID = 'API190819001';

select * from apec_staff a where a.staff_ID = 'API190819001';

select * from apec_hr_ a where a.staff_ID = 'API190819001';

select * from defmodbtn a where a.modid = '03444';

select * from modexecproc a where a.modid = '01444';

select * from modmaintain a where a.modid = '02G01';

SELECT * FROM modsearch A WHERE A.MODID = '03211';

select * from defmodfld a where a.modid = '03211';

select * from gdt_email_template a where a.title_template like '%Phi?u thu%';

select * from sys_email_server;
SELECT * FROM sys_email_server a WHERE a.email_service = 'AP_CRM';
--PKG_APEC_HIRING.SP_INVITE_INTERVIEW

SELECT * FROM apec_crm_receipt;

select * from defcode a where a.cdtype like '%CERTI%';
