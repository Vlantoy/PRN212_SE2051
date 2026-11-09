# -*- coding: utf-8 -*-
"""
Tập lệnh chính tạo tài liệu Word hoàn chỉnh (.docx):
Tai_Lieu_Chi_Tiet_ASM1_Va_WPF_App.docx
"""

import os
import docx
from docx.shared import Inches, Pt, RGBColor
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.enum.table import WD_TABLE_ALIGNMENT
from docx.oxml import OxmlElement
from docx.oxml.ns import qn

from build_docx import (
    set_cell_background, set_cell_margins, set_table_borders,
    add_header_styled, add_paragraph, add_bullet, 
    add_code_block, add_callout, add_table_styled
)

from doc_chapter1 import build_chapter_1
from doc_chapter2 import build_chapter_2
from doc_chapter3 import build_chapter_3
from doc_chapter4 import build_chapter_4

def create_full_document():
    doc = docx.Document()
    
    # Thiết lập lề trang tiêu chuẩn 1 inch (2.54 cm)
    for section in doc.sections:
        section.top_margin = Inches(1.0)
        section.bottom_margin = Inches(1.0)
        section.left_margin = Inches(1.0)
        section.right_margin = Inches(1.0)
        section.page_width = Inches(8.27)  # A4
        section.page_height = Inches(11.69)
        
    # Thiết lập Style Normal mặc định
    style_normal = doc.styles['Normal']
    font = style_normal.font
    font.name = 'Segoe UI'
    font.size = Pt(10.5)
    font.color.rgb = RGBColor(30, 41, 59)
    
    # ==================== TRANG BÌA (COVER HEADER) ====================
    title_p = doc.add_paragraph()
    title_p.alignment = WD_ALIGN_PARAGRAPH.CENTER
    title_p.paragraph_format.space_before = Pt(30)
    title_p.paragraph_format.space_after = Pt(8)
    
    run_title = title_p.add_run("GIÁO TRÌNH KỸ THUẬT CHUYÊN SÂU\nLÀM CHỦ C# .NET 8 & ỨNG DỤNG WPF")
    run_title.bold = True
    run_title.font.name = "Segoe UI"
    run_title.font.size = Pt(22)
    run_title.font.color.rgb = RGBColor(15, 41, 74)
    
    sub_p = doc.add_paragraph()
    sub_p.alignment = WD_ALIGN_PARAGRAPH.CENTER
    sub_p.paragraph_format.space_before = Pt(0)
    sub_p.paragraph_format.space_after = Pt(20)
    run_sub = sub_p.add_run("Phân tích bóc tách từng dòng code, nguyên lý CLR, luồng dữ liệu & kiến trúc phần mềm\ncho Bài tập Console ASM1 (Delegate) và Hệ thống Unified AppManager (WPF 4-in-1)")
    run_sub.font.name = "Segoe UI"
    run_sub.font.size = Pt(11.5)
    run_sub.font.italic = True
    run_sub.font.color.rgb = RGBColor(71, 85, 105)
    
    # Khung metadata trang bìa
    meta_headers = ["Thuộc tính", "Thông tin chi tiết"]
    meta_data = [
        ["Học phần", "PRN212 - Basic Cross-Platform Application Programming With .NET"],
        ["Nền tảng công nghệ", "C# 12, .NET 8.0, WPF (Windows Presentation Foundation)"],
        ["Công nghệ dữ liệu", "ADO.NET thuần (System.Data.SqlClient) & Entity Framework Core 8"],
        ["Cơ sở dữ liệu", "Microsoft SQL Server (SE1963, EmployeeDB, CourseAttendanceDB)"],
        ["Kiến trúc phần mềm", "Mô hình 3 lớp (3-Layer), Single-Window Host, In-memory View Caching, MVVM"],
        ["Mục tiêu tài liệu", "Phân tích chi tiết từng cú pháp, nguyên lý hoạt động giúp người học tự tin làm chủ và bảo vệ xuất sắc đồ án"]
    ]
    add_table_styled(doc, meta_headers, meta_data, col_widths=[2.0, 4.8])
    
    add_callout(doc, "Tuyên ngôn chất lượng",
        "Tài liệu này được biên soạn với độ chi tiết tối đa, giải thích tường tận từ bản chất bộ nhớ máy tính "
        "(Stack, Heap, Garbage Collection, CPU Execution), cơ chế luồng WPF (STA, UI Dispatcher vs ThreadPool), "
        "đến từng dòng mã nguồn của Console App ASM1 và ứng dụng WPF AppManager 4 màn hình. "
        "Tất cả mã nguồn đều đã được kiểm thử thực tế và biên dịch 0 lỗi trong solution SE2051.sln.",
        "tip"
    )
    
    doc.add_page_break()
    
    # ==================== MỤC LỤC TỔNG QUAN ====================
    add_header_styled(doc, "MỤC LỤC TỔNG QUAN HỆ THỐNG", level=1)
    
    toc_headers = ["Chương", "Nội dung trọng tâm", "Phạm vi mã nguồn phân tích"]
    toc_data = [
        [
            "CHƯƠNG 1",
            "Nền tảng kiến trúc .NET 8, CLR, Memory và WPF",
            "Chu trình JIT, Stack vs Managed Heap, IDisposable using var, Quy tắc UI Thread & Dispatcher, Đối sánh ADO.NET vs EF Core 8."
        ],
        [
            "CHƯƠNG 2",
            "Bóc tách từng dòng code bài tập ASM1 (Console & Delegate)",
            "Bản chất MulticastDelegate, Employee.cs, SalaryCaculation.cs, EmployeeList.cs (File I/O & LINQ), Program.cs, employees.txt."
        ],
        [
            "CHƯƠNG 3",
            "Bóc tách từng dòng code dự án AppManager (WPF 4-in-1)",
            "MainWindow Shell Container, View 1 Student Registration, View 2 Student Management (ADO.NET), View 3 Employee Management (EF Core 8), View 4 Attendance Management (3-Layer & MVVM), appsettings.json."
        ],
        [
            "CHƯƠNG 4",
            "Bảng tra cứu cú pháp, xử lý lỗi Runtime & Checklist thi cử",
            "Cheat Sheet C# 12, Bảng tra cứu lỗi Runtime kinh điển và cách sửa triệt để, Checklist 10 bước vàng trước giờ bảo vệ đồ án."
        ]
    ]
    add_table_styled(doc, toc_headers, toc_data, col_widths=[1.2, 2.8, 2.8])
    
    doc.add_page_break()
    
    # ==================== XÂY DỰNG 4 CHƯƠNG ====================
    print("Building Chapter 1...")
    build_chapter_1(doc)
    doc.add_page_break()
    
    print("Building Chapter 2...")
    build_chapter_2(doc)
    doc.add_page_break()
    
    print("Building Chapter 3...")
    build_chapter_3(doc)
    doc.add_page_break()
    
    print("Building Chapter 4...")
    build_chapter_4(doc)
    
    # ==================== LƯU TÀI LIỆU ====================
    output_filename = "Tai_Lieu_Chi_Tiet_ASM1_Va_WPF_App.docx"
    doc.save(output_filename)
    print(f"Document successfully saved as: {output_filename}")
    
    # In kích thước file
    size_bytes = os.path.getsize(output_filename)
    print(f"File size: {size_bytes / 1024:.2f} KB ({size_bytes} bytes)")

if __name__ == "__main__":
    create_full_document()
