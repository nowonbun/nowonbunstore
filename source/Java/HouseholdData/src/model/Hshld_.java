package model;

import java.math.BigDecimal;
import java.util.Date;
import javax.annotation.Generated;
import javax.persistence.metamodel.SingularAttribute;
import javax.persistence.metamodel.StaticMetamodel;

@Generated(value="Dali", date="2017-04-27T00:17:00.379+0900")
@StaticMetamodel(Hshld.class)
public class Hshld_ {
	public static volatile SingularAttribute<Hshld, Integer> ndx;
	public static volatile SingularAttribute<Hshld, String> cntxt;
	public static volatile SingularAttribute<Hshld, Date> dt;
	public static volatile SingularAttribute<Hshld, Date> pdt;
	public static volatile SingularAttribute<Hshld, BigDecimal> prc;
	public static volatile SingularAttribute<Hshld, UsrNf> usrNf;
	public static volatile SingularAttribute<Hshld, Tp> tpBean;
	public static volatile SingularAttribute<Hshld, Ctgry> ctgry;
}
