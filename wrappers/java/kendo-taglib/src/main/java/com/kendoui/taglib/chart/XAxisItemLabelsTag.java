
package com.kendoui.taglib.chart;


import com.kendoui.taglib.BaseTag;
import com.kendoui.taglib.json.Function;






import javax.servlet.jsp.JspException;

@SuppressWarnings("serial")
public class XAxisItemLabelsTag extends  BaseTag  /* interfaces *//* interfaces */ {
    
    @Override
    public int doEndTag() throws JspException {
//>> doEndTag


        XAxisItemTag parent = (XAxisItemTag)findParentWithClass(XAxisItemTag.class);


        parent.setLabels(this);

//<< doEndTag

        return super.doEndTag();
    }

    @Override
    public void initialize() {
//>> initialize
//<< initialize

        super.initialize();
    }

    @Override
    public void destroy() {
//>> destroy
//<< destroy

        super.destroy();
    }

//>> Attributes

    public static String tagName() {
        return "chart-xAxisItem-labels";
    }

    public void setBorder(com.kendoui.taglib.chart.XAxisItemLabelsBorderTag value) {
        setProperty("border", value);
    }

    public void setDateFormats(com.kendoui.taglib.chart.XAxisItemLabelsDateFormatsTag value) {
        setProperty("dateFormats", value);
    }

    public void setMargin(com.kendoui.taglib.chart.XAxisItemLabelsMarginTag value) {
        setProperty("margin", value);
    }

    public void setPadding(com.kendoui.taglib.chart.XAxisItemLabelsPaddingTag value) {
        setProperty("padding", value);
    }

    public void setRotation(com.kendoui.taglib.chart.XAxisItemLabelsRotationTag value) {
        setProperty("rotation", value);
    }

    public void setTemplate(XAxisItemLabelsTemplateFunctionTag value) {
        setEvent("template", value.getBody());
    }

    public void setVisual(XAxisItemLabelsVisualFunctionTag value) {
        setEvent("visual", value.getBody());
    }

    public java.lang.String getBackground() {
        return (java.lang.String)getProperty("background");
    }

    public void setBackground(java.lang.String value) {
        setProperty("background", value);
    }

    public java.lang.String getColor() {
        return (java.lang.String)getProperty("color");
    }

    public void setColor(java.lang.String value) {
        setProperty("color", value);
    }

    public java.lang.String getCulture() {
        return (java.lang.String)getProperty("culture");
    }

    public void setCulture(java.lang.String value) {
        setProperty("culture", value);
    }

    public java.lang.String getFont() {
        return (java.lang.String)getProperty("font");
    }

    public void setFont(java.lang.String value) {
        setProperty("font", value);
    }

    public java.lang.String getFormat() {
        return (java.lang.String)getProperty("format");
    }

    public void setFormat(java.lang.String value) {
        setProperty("format", value);
    }

    public float getMargin() {
        return (Float)getProperty("margin");
    }

    public void setMargin(float value) {
        setProperty("margin", value);
    }

    public boolean getMirror() {
        return (Boolean)getProperty("mirror");
    }

    public void setMirror(boolean value) {
        setProperty("mirror", value);
    }

    public float getPadding() {
        return (Float)getProperty("padding");
    }

    public void setPadding(float value) {
        setProperty("padding", value);
    }

    public java.lang.Object getRotation() {
        return (java.lang.Object)getProperty("rotation");
    }

    public void setRotation(java.lang.Object value) {
        setProperty("rotation", value);
    }

    public float getSkip() {
        return (Float)getProperty("skip");
    }

    public void setSkip(float value) {
        setProperty("skip", value);
    }

    public float getStep() {
        return (Float)getProperty("step");
    }

    public void setStep(float value) {
        setProperty("step", value);
    }

    public java.lang.String getTemplate() {
        return (java.lang.String)getProperty("template");
    }

    public void setTemplate(java.lang.String value) {
        setProperty("template", value);
    }

    public boolean getVisible() {
        return (Boolean)getProperty("visible");
    }

    public void setVisible(boolean value) {
        setProperty("visible", value);
    }

    public String getVisual() {
        Function property = ((Function)getProperty("visual"));
        if (property != null) {
            return property.getBody();
        }
        return null;
    }

    public void setVisual(String value) {
        setProperty("visual", new Function(value));
    }

//<< Attributes

}