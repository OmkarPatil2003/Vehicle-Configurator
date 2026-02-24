package com.example.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.example.dto.InvoiceRequestDTO;
import com.example.service.Invoicemanager;

@RestController
@RequestMapping("/api/invoice")
public class InvoiceController {

    @Autowired
    private Invoicemanager invoiceService;

    @Autowired
    private com.example.repository.UserRepository userRepository;

    @PostMapping("/confirm")
    public ResponseEntity<String> confirmOrder(@RequestBody InvoiceRequestDTO dto) {

        System.out.println("DEBUG: >>> InvoiceController.confirmOrder HIT <<<");
        System.out.println("DEBUG: Request received for User ID: " + dto.getUserId());

        if (dto.getUserId() == null) {
            return ResponseEntity.badRequest().body("User ID is required");
        }

        // Generate invoice using the passed User ID
        invoiceService.generateInvoice(dto);

        return ResponseEntity.ok("Invoice generated successfully by Invoice Microservice");
    }
}
