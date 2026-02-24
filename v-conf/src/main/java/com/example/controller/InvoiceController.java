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
        // MICROSERVICE ARCHITECTURE FLOW
        // ---------------------------------------------------------
        try {
            String microserviceUrl = "http://localhost:9003/api/invoice/confirm";
            System.out.println("Calling Invoice Microservice at: " + microserviceUrl);

            String response = restTemplate.postForObject(
                    microserviceUrl,
                    dto,
                    String.class);

            return ResponseEntity.ok(response);

        } catch (Exception ex) {
            System.err.println("Failed to call Invoice Microservice: " + ex.getMessage());
            ex.printStackTrace();

            return ResponseEntity.status(500).body("Error Generating Invoice via Microservice: " + ex.getMessage());

            // ---------------------------------------------------------
            // LEGACY / MONOLITIC FALLBACK FLOW (Reference Only)
            // ---------------------------------------------------------
            /*
             * System.out.println("Switching to Legacy Monolithic Logic...");
             * invoiceService.generateInvoice(dto);
             * return ResponseEntity.ok("Invoice generated (Legacy Fallback)");
             */
        }
    }
}
