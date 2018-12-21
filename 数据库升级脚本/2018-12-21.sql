---2018-12-21

/*�����γ���Ƶ�γ��ֶεĳ���*/
alter table [Outline] ALTER COLUMN  Ol_Courseware [nvarchar](max) NULL
go
alter table [Outline] ALTER COLUMN  Ol_Vedio [nvarchar](max) NULL
go
alter table [Outline] ALTER COLUMN  Ol_LessonPlan [nvarchar](max) NULL
go
/*�����ֶ�������ǰ����ƴ���ˣ�*/
execute sp_rename 'Outline.Ol_Vedio','Ol_Video'
go
/*�����½ڽڵ���ֶΣ�������¼��½ڣ���ǰ�½�Ϊ�ڵ�*/
alter table [Outline] add Ol_IsNode bit NULL
go
UPDATE [Outline]  SET Ol_IsNode = 0
GO
alter table [Outline] ALTER COLUMN Ol_IsNode bit NOT NULL
go
/*�����½ڵ��Ƿ�����Ƶ���ֶ�*/
alter table [Outline] add Ol_IsVideo bit NULL
go
UPDATE [Outline]  SET Ol_IsVideo = 0
GO
alter table [Outline] ALTER COLUMN Ol_IsVideo bit NOT NULL
go
/*����½����¼��½ڣ���ǰ�½�Ϊ�½ڽڵ㣬������Ϊѧϰ�½�*/
declare cursor_obj  cursor scroll
for select ol_id,ol_name from outline
open cursor_obj
declare @olid int, @olname nvarchar(500), @child int
set @child=0
fetch First from cursor_obj into @olid,@olname
while @@fetch_status=0  --��ȡ�ɹ���������һ�����ݵ���ȡ���� 
 begin
   select @child=COUNT(*) from outline where Ol_PID=@olid
   if @child>0
   begin
	--������¼�
	--print convert(varchar(20),@olid)+' '+@olname +' �¼�����' +convert(varchar(20),@child)	
	Update outline Set Ol_IsNode=1 Where ol_id=@olid  --�޸ĵ�ǰ��
   end
   else
   begin
	--û���¼��½�
	Update outline Set Ol_IsNode=0 Where ol_id=@olid  --�޸ĵ�ǰ��
   end
   
   fetch next from cursor_obj into @olid, @olname   --�ƶ��α�
 end   
--�رղ��ͷ��α�
close cursor_obj
deallocate cursor_obj

/*����½�����Ƶ������Ϊ��Ƶ�½�*/
declare cursor_obj  cursor scroll
for select ol_id,ol_uid from outline
open cursor_obj
declare @olid2 int, @uid nvarchar(500), @child2 int
set @child2=0
fetch First from cursor_obj into @olid2,@uid
while @@fetch_status=0  --��ȡ�ɹ���������һ�����ݵ���ȡ���� 
 begin
   select @child2=COUNT(*) from Accessory where As_Type='CourseVideo' and  As_Uid=@uid 
   if @child2>0
   begin
	--�������Ƶ
	--print convert(varchar(20),@olid2)+' '+@uid +' ��Ƶ����' +convert(varchar(20),@child2)	
	Update outline Set Ol_IsVideo=1 Where ol_id=@olid2  --�޸ĵ�ǰ��
   end
   else
   begin
	--û����Ƶ
	--print convert(varchar(20),@olid2)+' '+@uid +'  ' +convert(varchar(20),@child2)	
	Update outline Set Ol_IsVideo=0 Where ol_id=@olid2  --�޸ĵ�ǰ��
  	DELETE FROM [LogForStudentStudy] where ol_id=@olid2 
   end
   
   fetch next from cursor_obj into @olid2, @uid   --�ƶ��α�
 end   
--�رղ��ͷ��α�
close cursor_obj
deallocate cursor_obj
/**/
/*������½ڲ����ڣ���ɾ�����½�*/
declare cursor_obj  cursor scroll
for select ol_id,ol_name,ol_pid from outline where Ol_PID!=0 
open cursor_obj
declare @olid3 int, @name nvarchar(500),@pid int, @child3 int, @num int
set @child3=0
set @num=0
fetch First from cursor_obj into @olid3,@name,@pid
while @@fetch_status=0  --��ȡ�ɹ���������һ�����ݵ���ȡ���� 
 begin
   select @child3=COUNT(*) from outline where Ol_ID=@pid 
   if @child3<1
   begin
	--������½ڲ����ڣ���ɾ�����½�
	--print convert(varchar(20),@olid)+ @name +convert(varchar(20),@child2)	
	DELETE FROM [LogForStudentStudy] where ol_id=@olid3 
	DELETE FROM outline where ol_id=@olid3 
	select @num=@num+1
   end   
   
   fetch next from cursor_obj into @olid3, @name,@pid   --�ƶ��α�
 end   
--�رղ��ͷ��α�
close cursor_obj
deallocate cursor_obj

print @num