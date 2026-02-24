package com.example.dto;
import java.util.*;
public class InvoiceRequestDTO {

	 private Integer userId;
	    private Integer modelId;
	    private Integer qty;
	    private String customerDetail;
		private List<Integer> alternates;
		public List<Integer> getAlternates() {
			return alternates;
		}
		public void setAlternates(List<Integer> alternates) {
			this.alternates = alternates;
		}
		public Integer getUserId() {
			return userId;
		}
		public void setUserId(Integer userId) {
			this.userId = userId;
		}
		public Integer getModelId() {
			return modelId;
		}
		public void setModelId(Integer modelId) {
			this.modelId = modelId;
		}
		public Integer getQty() {
			return qty;
		}
		public void setQty(Integer qty) {
			this.qty = qty;
		}
		public String getCustomerDetail() {
			return customerDetail;
		}
		public void setCustomerDetail(String customerDetail) {
			this.customerDetail = customerDetail;
		}

}
