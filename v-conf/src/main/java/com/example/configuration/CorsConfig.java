package com.example.configuration;

import org.springframework.context.annotation.Configuration;
import org.springframework.web.servlet.config.annotation.CorsRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

@Configuration
public class CorsConfig implements WebMvcConfigurer {

        @org.springframework.beans.factory.annotation.Value("${frontend.url}")
        private String frontendUrl;

        @Override
        public void addCorsMappings(CorsRegistry registry) {

                String[] allowedOrigins = frontendUrl.split(",");

                registry.addMapping("/**")
                                .allowedOrigins(allowedOrigins)
                                .allowedMethods(
                                                "GET",
                                                "POST",
                                                "PUT",
                                                "DELETE",
                                                "OPTIONS")
                                .allowedHeaders("*")
                                .allowCredentials(true);
        }
}