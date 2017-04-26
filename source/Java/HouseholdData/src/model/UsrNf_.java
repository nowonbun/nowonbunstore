package model;

import java.util.Date;
import javax.annotation.Generated;
import javax.persistence.metamodel.ListAttribute;
import javax.persistence.metamodel.SingularAttribute;
import javax.persistence.metamodel.StaticMetamodel;

@Generated(value="Dali", date="2017-04-27T00:17:00.412+0900")
@StaticMetamodel(UsrNf.class)
public class UsrNf_ {
	public static volatile SingularAttribute<UsrNf, String> id;
	public static volatile SingularAttribute<UsrNf, Date> createdate;
	public static volatile SingularAttribute<UsrNf, String> email;
	public static volatile SingularAttribute<UsrNf, String> name;
	public static volatile ListAttribute<UsrNf, Hshld> hshlds;
	public static volatile ListAttribute<UsrNf, HshldRelation> hshldRelations1;
	public static volatile ListAttribute<UsrNf, HshldRelation> hshldRelations2;
	public static volatile ListAttribute<UsrNf, TknDmn> tknDmns;
}
