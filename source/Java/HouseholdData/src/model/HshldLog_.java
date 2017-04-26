package model;

import java.math.BigDecimal;
import java.util.Date;
import javax.annotation.Generated;
import javax.persistence.metamodel.SingularAttribute;
import javax.persistence.metamodel.StaticMetamodel;

@Generated(value="Dali", date="2017-04-27T00:17:00.387+0900")
@StaticMetamodel(HshldLog.class)
public class HshldLog_ {
	public static volatile SingularAttribute<HshldLog, Integer> ndx;
	public static volatile SingularAttribute<HshldLog, String> cd;
	public static volatile SingularAttribute<HshldLog, String> cntxt;
	public static volatile SingularAttribute<HshldLog, Date> dt;
	public static volatile SingularAttribute<HshldLog, String> id;
	public static volatile SingularAttribute<HshldLog, Integer> ndx2;
	public static volatile SingularAttribute<HshldLog, Date> pdt;
	public static volatile SingularAttribute<HshldLog, BigDecimal> prc;
	public static volatile SingularAttribute<HshldLog, String> tp;
}
