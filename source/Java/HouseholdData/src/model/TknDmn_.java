package model;

import java.util.Date;
import javax.annotation.Generated;
import javax.persistence.metamodel.SingularAttribute;
import javax.persistence.metamodel.StaticMetamodel;

@Generated(value="Dali", date="2017-04-27T00:17:00.402+0900")
@StaticMetamodel(TknDmn.class)
public class TknDmn_ {
	public static volatile SingularAttribute<TknDmn, Integer> ndx;
	public static volatile SingularAttribute<TknDmn, String> accessToken;
	public static volatile SingularAttribute<TknDmn, Date> applyDate;
	public static volatile SingularAttribute<TknDmn, UsrNf> usrNf;
}
