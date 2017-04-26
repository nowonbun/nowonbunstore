package model;

import javax.annotation.Generated;
import javax.persistence.metamodel.ListAttribute;
import javax.persistence.metamodel.SingularAttribute;
import javax.persistence.metamodel.StaticMetamodel;

@Generated(value="Dali", date="2017-04-27T00:17:00.134+0900")
@StaticMetamodel(Ctgry.class)
public class Ctgry_ {
	public static volatile SingularAttribute<Ctgry, String> cd;
	public static volatile SingularAttribute<Ctgry, String> nm;
	public static volatile ListAttribute<Ctgry, Hshld> hshlds;
	public static volatile ListAttribute<Ctgry, Tp> tps;
}
