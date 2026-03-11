package com.example.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.client.RestTemplate;

import com.example.dto.InvoiceRequestDTO;
import com.example.service.Invoicemanager;

@RestController
@RequestMapping("/api/invoice")
public class InvoiceController {

    @Autowired
    private RestTemplate restTemplate;

    @Autowired
    private Invoicemanager invoiceService;

    @Autowired
    private com.example.repository.UserRepository userRepository;

    @PostMapping("/confirm")
    public ResponseEntity<String> confirmOrder(
            @RequestBody InvoiceRequestDTO dto,
            org.springframework.security.core.Authentication authentication) {

        if (authentication == null) {
            return ResponseEntity.status(401).body("Unauthorized");
        }

        String username = authentication.getName();
        com.example.models.User user = userRepository.findByUsername(username)
                .orElseThrow(() -> new RuntimeException("User not found"));

        // Force the user ID to be the authenticated user's ID
        dto.setUserId(user.getId());

        // ---------------------------------------------------------
        // MONOLITHIC ARCHITECTURE FLOW
        // ---------------------------------------------------------
        try {
            System.out.println("Switching to Legacy Monolithic Logic...");
            invoiceService.generateInvoice(dto);
            return ResponseEntity.ok("Invoice generated (Legacy Fallback)");
        } catch (Exception ex) {
            System.err.println("Failed to generate invoice: " + ex.getMessage());
            ex.printStackTrace();
            return ResponseEntity.status(500).body("Error Generating Invoice: " + ex.getMessage());
        }
    }
}
