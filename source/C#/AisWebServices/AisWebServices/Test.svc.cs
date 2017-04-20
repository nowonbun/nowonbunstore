using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AisWebServices;
using AisWebServicesDao;

namespace AisWebServices
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "Test"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 Test.svc나 Test.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class Test : Allocation,ITest
    {
        [ResourceDao]
        private ITestDBDao testDbDao;

        public String DoWork()
        {
            IList<AisWebServicesDao.TestDB> ret = testDbDao.select();
            return AIsSocketServices.AisSocketServices.GetInstance().SetDataList<AisWebServicesDao.TestDB>(ret);
        }
    }
}
