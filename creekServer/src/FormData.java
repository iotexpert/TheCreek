package org.elkhorncreek;

public class FormData {

    private String Sval=new String();
    private int count=0;

    

    public String getSval()
    {
	return Sval;
    }

    public void setSval(String s)
    {
	Sval = s;
    }
    

    public int getcount()
    {
	return count;
    }

    public void setcount(int c)
    {
	count = c;
    }

    public int inccount()
    {
	count = count + 1;
	return count;
    }

    

}