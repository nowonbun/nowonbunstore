package Entity;

import javax.json.Json;
import javax.json.JsonObject;
import javax.json.JsonObjectBuilder;
import java.text.SimpleDateFormat;

import Common.Util;
import entity.CommonData;
import entity.PackageData;
import entity.ResultData;

public class ScrapNode {

	private int mallkey;
	private String key;
	private ResultType resulttype;
	private CommonData commondata = null;
	private PackageData packagedata = null;
	private ResultData resultdata = null;

	public ScrapNode(int mallkey, String key) {
		this.mallkey = mallkey;
		this.key = key;
	}

	public String getKey() {
		return key;
	}

	public int getMallkey() {
		return mallkey;
	}

	public ResultType getResultType() {
		return resulttype;
	}

	public void setResultType(int resulttype) {
		if (resulttype == 0) {
			this.resulttype = ResultType.Common;
		} else if (resulttype == 1) {
			this.resulttype = ResultType.Pacakage;
		} else if (resulttype == 2) {
			this.resulttype = ResultType.Exit;
		} else if (resulttype == 3) {
			this.resulttype = ResultType.Error;
		}
	}

	public void setResultType(ResultType resulttype) {
		this.resulttype = resulttype;
	}

	public CommonData getCommonData() {
		return commondata;
	}

	public PackageData getPackageData() {
		return packagedata;
	}

	public ResultData getResultData() {
		return resultdata;
	}
	
	public String getResultJson() {
		SimpleDateFormat transFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss.SSS");
		
		JsonObjectBuilder builder = Json.createObjectBuilder();
		addJson(builder, "MallKey", getMallkey());
		addJson(builder, "Key", getKey());
		addJson(builder, "ResultCD", getResultData().getResultcd());
		addJson(builder, "ResultMSG", getResultData().getResultmsg());
		addJson(builder, "Starttime", transFormat.format(getResultData().getStarttime()));
		addJson(builder, "Endtime", transFormat.format(getResultData().getEndtime()));
		return builder.build().toString();
	}

	private JsonObjectBuilder addJson(JsonObjectBuilder builder, String name, int value) {
		builder.add(name, value);
		return builder;
	}

	private JsonObjectBuilder addJson(JsonObjectBuilder builder, String name, String value) {
		if (value == null) {
			builder.addNull(name);
		} else {
			builder.add(name, value);
		}
		return builder;
	}

	/**
	 * 공통데이터 (회원정보)
	 * @param object JsonObject
	 */
	public void setCommonData(JsonObject object) {
		commondata = new CommonData(mallkey, key, object.getInt("Index"));
		if (!object.isNull("Data")) {
			commondata.setData(object.getString("Data"));
		}
	}

	/**
	 * 공통데이터 외 (매출내역,정산내역,정산예정금,반품율 등)
	 * @param object JsonObject
	 */
	public void setPackageData(JsonObject object) {
		packagedata = new PackageData(mallkey, key, object.getInt("Index"), object.getInt("Separation"));
		if (!object.isNull("Data")) {
			packagedata.setData(object.getString("Data"));
		}
	}

	/**
	 * 결과데이터
	 * @param object JsonObject
	 */
	public void setResultData(JsonObject object) {
		resultdata = new ResultData(mallkey, key);
		if (!object.isNull("ResultCD")) {
			resultdata.setResultcd(object.getString("ResultCD"));
		}
		if (!object.isNull("ResultMSG")) {
			resultdata.setResultmsg(object.getString("ResultMSG"));
		}
		if (!object.isNull("Starttime")) {
			resultdata.setStarttime(Util.ConvertToDateFromString(object.getString("Starttime")));
		}
		if (!object.isNull("Endtime")) {
			resultdata.setEndtime(Util.ConvertToDateFromString(object.getString("Endtime")));
		}
	}
}
