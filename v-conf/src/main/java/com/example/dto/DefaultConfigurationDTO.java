package com.example.dto;

public class DefaultConfigurationDTO {
	private Integer id;
	private String name;

	public DefaultConfigurationDTO(Integer id, String name, String componentName) {
		this.id = id;
		this.name = name;
		this.componentName = componentName;
	}

	private String componentName;

	public Integer getId() {
		return id;
	}

	public void setId(Integer id) {
		this.id = id;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getComponentName() {
		return componentName;
	}

	public void setComponentName(String componentName) {
		this.componentName = componentName;
	}
}
