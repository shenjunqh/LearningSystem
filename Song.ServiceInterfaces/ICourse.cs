using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// �γ̹���
    /// </summary>
    public interface ICourse : WeiSha.Common.IBusinessInterface
    {
        #region �γ̹���
        /// <summary>
        /// ��ӿγ�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void CourseAdd(Course entity);
        /// <summary>
        /// ������ӿγ̣������ڵ���ʱ
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="names">���ƣ��������ö��ŷָ��Ķ������</param>
        /// <returns></returns>
        Course CourseBatchAdd(int orgid, int sbjid, string names);
        /// <summary>
        /// �Ƿ��Ѿ����ڿγ�
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="pid">�ϼ�id</param>
        /// <param name="name"></param>
        /// <returns></returns>
        Course CourseIsExist(int orgid, int sbjid, int pid, string name);
        /// <summary>
        /// �޸Ŀγ�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void CourseSave(Course entity);
        /// <summary>
        /// ɾ���γ�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void CourseDelete(Course entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void CourseDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Course CourseSingle(int identify);
        /// <summary>
        /// ��ȡ�γ����ƣ����Ϊ�༶������ϸ�������
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        string CourseName(int identify);       
        /// <summary>
        /// ��ȡ���пγ�
        /// </summary>
        /// <param name="orgid">���ڻ���id</param>
        /// <param name="thid">��ʦid</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        List<Course> CourseAll(int orgid, int sbjid, int thid, bool? isUse);
        /// <summary>
        /// ĳ���γ̵�ѧϰ����
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="isAll">�Ƿ�ȡȫ��ֵ�����Ϊfalse�����ȡ��ǰ����ѧϰ��</param>
        /// <returns></returns>
        int CourseStudentSum(int couid, bool? isAll);
        /// <summary>
        /// ����γ̵�����
        /// </summary>
        /// <param name="identify"></param>
        void CourseClear(int identify);
        /// <summary>
        /// ��ȡָ�������Ŀγ��б�
        /// </summary>
        /// <param name="orgid">���ڻ���id</param>
        /// <param name="sbjid">רҵid������0ȡ����</param>
        /// <param name="thid">��ʦid</param>
        /// <param name="pid">�ϼ��γ�ID</param>
        /// <param name="isUse"></param>
        /// <param name="count">ȡ��������¼�����С�ڵ���0����ȡ����</param>
        /// <returns></returns>
        List<Course> CourseCount(int orgid, int sbjid, int thid, int pid, string sear, bool? isUse, int count);
        List<Course> CourseCount(int orgid, int sbjid, string sear, bool? isUse, int count);
        /// <summary>
        /// ��ȡָ�������Ŀγ��б�
        /// </summary>
        /// <param name="orgid">���ڻ���id</param>
        /// <param name="sbjid">רҵid������0ȡ����</param>
        /// <param name="sear"></param>
        /// <param name="isUse"></param>
        /// <param name="order">����ʽ��def:Ĭ�ϣ����Ƽ���Ȼ�󰴷���������;flux��������������;tax�����Զ�������Ҫ��;new:������ʱ�䣬���·�����ǰ��;rec:���Ƽ������Ƽ���Ȼ��tax����</param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Course> CourseCount(int orgid, int sbjid, string sear, bool? isUse, string order, int count);
        /// <summary>
        /// �γ�����
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="thid">��ʦid</param>
        /// <returns></returns>
        int CourseOfCount(int orgid, int sbjid, int thid);
        /// <summary>
        /// ��ǰ�γ����Ƿ����Ӽ�
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        bool CourseIsChildren(int orgid, int couid, bool? isUse);
        /// <summary>
        /// ��ҳ��ȡ
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="thid"></param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Course> CoursePager(int orgid, int thid, bool? isUse, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡ
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid"></param>
        /// <param name="thid"></param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="order">����ʽ��Ĭ��null������˳��flux�����������</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Course> CoursePager(int orgid, int sbjid, int thid, bool? isUse, string searTxt, string order, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡ
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid">רҵid,���id�ö��ŷָ�</param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="order">����ʽ��def:Ĭ�ϣ����Ƽ���Ȼ�󰴷���������;flux��������������;tax�����Զ�������Ҫ��;new:������ʱ�䣬���·�����ǰ��;rec:���Ƽ������Ƽ���Ȼ��tax����</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Course> CoursePager(int orgid, string sbjid, bool? isUse, string searTxt, string order, int size, int index, out int countSum);
        /// <summary>
        /// ����ǰ��Ŀ�����ƶ������ڵ�ǰ�����ͬ���ƶ�����ͬһ���ڵ��µĶ�����ǰ�ƶ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns>����Ѿ����ڶ��ˣ��򷵻�false���ƶ��ɹ�������true</returns>
        bool CourseUp(int id);
        /// <summary>
        /// ����ǰ��Ŀ�����ƶ������ڵ�ǰ�����ͬ���ƶ�����ͬһ���ڵ��µĶ�����ǰ�ƶ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns>����Ѿ����ڶ��ˣ��򷵻�false���ƶ��ɹ�������true</returns>
        bool CourseDown(int id);
        #endregion

        #region �γ̹���        
        /// <summary>
        /// ����γ�
        /// </summary>
        /// <param name="stc">ѧ����γ̵Ĺ�������</param>
        Student_Course Buy(Student_Course stc);
        /// <summary>
        /// ����γ�
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="price">�۸���</param>
        /// <returns></returns>
        Student_Course Buy(int stid, int couid, Song.Entities.CoursePrice price);
        /// <summary>
        /// ���ѧϰ
        /// </summary>
        /// <param name="stid">ѧϰID</param>
        /// <param name="couid">�γ�ID</param>
        /// <returns></returns>
        Student_Course FreeStudy(int stid, int couid);
        /// <summary>
        /// ���ѧϰ
        /// </summary>
        /// <param name="stid">ѧϰID</param>
        /// <param name="couid">�γ�ID</param>
        /// <param name="start">���ʱЧ�Ŀ�ʼʱ��</param>
        /// <param name="end">���ʱЧ�Ľ���ʱ��</param>
        /// <returns></returns>
        Student_Course FreeStudy(int stid, int couid, DateTime start, DateTime end);
        /// <summary>
        /// ѧԱ�Ƿ����˸ÿγ�
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="stid">ѧԱId</param>
        /// <param name="state">0�����Ƿ���ڣ�1�����ǹ���ʱЧ�ڵģ�2�����ǹ���ʱЧ���</param>
        /// <returns>���ؿγ�</returns>
        Course IsBuyCourse(int couid, int stid, int state);
        /// <summary>
        /// ѧԱ�Ƿ����˸ÿγ�
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="stid"></param>
        /// <param name="state">0�����Ƿ���ڣ�1�����ǹ���ʱЧ�ڵģ�2�����ǹ���ʱЧ���</param>
        /// <returns></returns>
        bool IsBuy(int couid, int stid, int state);
        /// <summary>
        /// �γ����ã�Ĭ������һ����
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="couid">�γ�id</param>
        Student_Course Tryout(int stid, int couid);
        /// <summary>
        /// �Ƿ����øÿγ�
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="stid"></param>
        /// <returns></returns>
        bool IsTryout(int couid, int stid);
        /// <summary>
        /// ֱ��ѧϰ�ÿγ�
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="stid">ѧԱid</param>
        /// <returns>�������ѻ���ʱ��ѡ�����ѧ�Ŀγ̣�����ѧϰ������true������ѧϰ����false</returns>
        bool Study(int couid, int stid);
        /// <summary>
        /// ѧ������γ̵ļ�¼��
        /// </summary>
        /// <param name="stid">ѧԱId</param>
        /// <param name="couid">�γ�id</param>
        /// <returns></returns>
        Student_Course StudyCourse(int stid, int couid);
        /// <summary>
        /// ȡ���γ�ѧϰ��ֱ��ɾ�������¼
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        void DelteCourseBuy(int stid, int couid);        
        /// <summary>
        /// ѧԱ����ĸÿγ�
        /// </summary>
        /// <param name="stid">ѧԱId</param>
        /// <param name="sear">���ڼ������ַ�</param>
        /// <param name="state">0�����Ƿ���ڣ�1�����ǹ���ʱЧ�ڵģ�2�����ǹ���ʱЧ���</param>
        /// <param name="istry">�Ƿ����ã�Ϊnullʱȡ����</param>
        /// <param name="count">ȡ������</param>
        /// <returns></returns>
        List<Course> CourseForStudent(int stid, string sear, int state, bool? istry, int count);
        #endregion

        #region �γ̹���������ѧ�����ʦ��        
        /// <summary>
        /// ��ȡѡѧ�������Ŀγ��б��Ӷൽ��
        /// </summary>
        /// <param name="orgid">����Id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="count">ȡ������</param>
        /// <returns></returns>
        DataSet CourseHot(int orgid, int sbjid, int count);
        /// <summary>
        /// ĳ��ѧ���Ƿ�����ѧϰĳ���γ�
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        bool StudyIsCourse(int stid, int couid);        
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="stid">ѧ��Id</param>
        /// <param name="couid">�γ�id</param>
        /// <returns></returns>
        void CourseBuy(int stid, int couid, float money, DateTime startTime, DateTime endTime); 
        /// <summary>
        /// ��ȡĳ����ʦ�����Ŀγ�
        /// </summary>
        /// <param name="thid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Course> Course4Teacher(int thid, int count);
        /// <summary>
        /// ѧϰĳ���γ̵�ѧԱ
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="stname"></param>
        /// <param name="stmobi"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Accounts[] Student4Course(int couid, string stname, string stmobi, int size, int index, out int countSum);
        #endregion

        #region �۸����
        /// <summary>
        /// ��Ӽ۸��¼
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void PriceAdd(CoursePrice entity);
        /// <summary>
        /// �޸ļ۸��¼
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void PriceSave(CoursePrice entity);
        /// <summary>
        /// ɾ���۸��¼
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void PriceDelete(CoursePrice entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void PriceDelete(int identify);
        /// <summary>
        /// ɾ������ȫ��Ψһ��ʶ
        /// </summary>
        /// <param name="uid"></param>
        void PriceDelete(string uid);
        /// <summary>
        /// ����Ʒ�۸�д�뵽�γ����ڵı�ȡ��һ���۸�
        /// </summary>
        /// <param name="uid">�γ�UID</param>
        void PriceSetCourse(string uid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        CoursePrice PriceSingle(int identify);
        /// <summary>
        /// ��ȡ�۸��¼
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="uid"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        CoursePrice[] PriceCount(int couid, string uid, bool? isUse, int count);
        /// <summary>
        /// ����ǰ��Ŀ�����ƶ������ڵ�ǰ�����ͬ���ƶ�����ͬһ���ڵ��µĶ�����ǰ�ƶ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns>����Ѿ����ڶ��ˣ��򷵻�false���ƶ��ɹ�������true</returns>
        bool PriceUp(int id);
        /// <summary>
        /// ����ǰ��Ŀ�����ƶ������ڵ�ǰ�����ͬ���ƶ�����ͬһ���ڵ��µĶ�����ǰ�ƶ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns>����Ѿ����ڶ��ˣ��򷵻�false���ƶ��ɹ�������true</returns>
        bool PriceDown(int id);
        #endregion
    }
}
