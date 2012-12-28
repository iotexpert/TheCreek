package org.elkhorncreek;

public class Depth {
	
	Float tot;
	Integer ft;
	Integer inches;
	
	public Float getTot() {
		return tot;
	}

	public void setTot(Float tot) {
		this.tot = tot;
	}

	public Integer getFt() {
		return ft;
	}

	public void setFt(Integer ft) {
		this.ft = ft;
	}

	public Integer getInches() {
		return inches;
	}

	public void setInches(Integer inches) {
		this.inches = inches;
	}


	Depth(Float v, Float zero)
	{
		tot = new Float(  ( (v-zero) / (51.1*(0.02 - 0.004 )) * 15 / 0.43));
		ft = tot.intValue();
		Float f = tot - new Float(ft);
		inches = f.intValue();
		
	}

}
