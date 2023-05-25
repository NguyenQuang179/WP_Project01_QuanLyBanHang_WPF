## PROJECT 1 - MyShop

### Thông tin nhóm

#### Thông tin thành viên
- Trương Gia Huy - 20127040 - tghuy201@clc.fitus.edu.vn
- Lưu Hoàng Minh - 20127045-- lhminh20@clc.fitus.edu.vn
- Nguyễn Ngọc Quang - 20127297 - nnquang20@clc.fitus.edu.vn
- Hà Tuấn Lâm - 20127677- htlam20@clc.fitus.edu.vn

#### Đề tài
- Xây dựng một chương trình quản lí sách, cũng như là phân loại và các đơn hàng đặt sách

### Yêu cầu cần thiết để chạy chương trình

#### Thông tin đăng nhập:
- Username: admin
- Password: 123456

#### Cài đặt ứng dụng:
1. Mở thư mục "HMQL_Bookstore_Manager".
2. Chạy setup.exe để mở Installer cho chương trình.
3. Sau khi installer hoàn thành cài đặt, chạy Shortcut được tạo ra ở desktop hoặc ở thư mục cài đặt.
4. Cung cấp quyền admin để chạy chương trình.

#### Chạy ứng dụng:
- Thông qua shortcut được tạo ở installer.
- Thông qua chạy Debug chương trình bên trong Solution.


### Các yêu cầu đã được thực hiện:

#### *_Yêu cầu thường:_*

- [x] **Màn hình đăng nhập. (0.5 điểm)**
- [x] **Màn hình dashboard. (0.5 điểm)**
- [x] **Quản lí hàng hóa.(1,5 điểm)**
  - [x] Import dữ liệu gốc ban đầu từ Excel
  - [x] Xem danh sách các sản phẩm theo loại có phân trang
  - [x] Cho phép thêm một loại sản phẩm, xóa một loại sản phẩm, hiệu chỉnh loại sản phẩm.
  - [x] Cho phép thêm một sản phẩm, xóa một sản phẩm, hiệu chỉnh thông tin sản phẩm.
  - [x] Cho phép tìm kiếm sản phẩm theo tên.
  - [x] Cho phép lọc lại sản phẩm theo khoảng giá.
- [x] **Quản lí các đơn hàng. (1.5 điểm)**
  - [x] Tạo ra các đơn hàng.
  - [x] Cho phép xóa một đơn hàng, cập nhật một đơn hàng.
  - [x] Cho phép xem danh sách các đơn hàng có phân trang, xem chi tiết một đơn hàng.
  - [x] Tìm kiếm các đơn hàng từ ngày đến ngày
- [x] **Báo cáo thống kê (2 điểm)**
  - [x] Báo cáo doanh thu và lợi nhuận theo ngày đến ngày, theo tuần, theo tháng, theo năm (vẽ biểu đồ)
  - [x] Xem các sản phẩm và số lượng bán thao ngày đến ngày, theo tuần, theo tuần, theo tháng, theo năm(vẽ biểu đồ)
- [x] **Cấu hình (0.25/0.5 điểm)**
  - [x] Hiệu chỉnh số lượng sản phẩm mỗi trang
  - [ ] Cho phép khi chạy chương trình lên thì mở lại màn hình cuối mà lần trước tắt
- [x] **Đóng gói thành file cài đặt (0.5 điểm)**

#### *_Yêu cầu nâng cao:_*
- [x] **Sử dụng một thiết kế giao diện tốt lấy từ pinterest(0.5 điểm)**
- [x] **Báo cáo các sản phẩm bán chạy trong tuần, trong tháng, trong năm (1 điểm)**
- [x] **Sử dụng mô hình MVVM (1 điểm)**
- [x] **Kết nối API Rest API (2 điểm)**

### Yêu cầu chưa hoàn thiện:
- [ ] Cho phép khi chạy chương trình lên thì mở lại màn hình cuối mà lần trước tắt

### Phân công công việc:
##### Trương Gia Huy:
- [x] Nhóm chức năng quản lí các đơn hàng.
    - Màn hình danh sách đơn hàng.
    - Màn hình thêm đơn hàng.
    - Màn hình chỉnh sửa/ chi tiết đơn hàng.
    - Màn hình thêm sách vào đơn hàng.
- [x] Nhóm chức năng quản lí các phân loại sản phẩm 
    - Màn hình thêm phân loại .
    - Màn hình chỉnh sửa/chi tiết phân loại.

##### Lưu Hoàng Minh:
- [x] Nhóm chức năng quản lí sản phẩm.
    - Màn hình danh sách sản phẩm.
    - Màn hình thêm sản phẩm.
    - Màn hình chỉnh sửa sản phẩm.
      
##### Nguyễn Ngọc Quang:
- [x] Xây dựng màn hình đăng nhập.
- [x] Xây dựng màn hình Dashboard.
- [x] Xây dựng màn hình xem danh sách phân loại.
- [x] Nhóm chức năng Báo cáo thống kê.
  -  Màn hình và biểu đồ báo cáo doanh thu.
  -  Màn hình và biểu đồ báo cáo sản phẩm bán được theo ngày/tháng/năm.

##### Hà Tuấn Lâm:
- [x] Thiết kế và xây dựng Back-end và Cơ sở dữ liệu.
- [x] Viết các API để lấy, thêm, cập nhật và xóa các loại dữ liệu trên database.
- [x] Xây dựng đóng gói file cài dặt.
- [x] Import dữ liệu gốc từ excel.
- [x] Viết các hàm để gọi API từ server cho chương trình.
### Điểm Đề Nghị
- Điểm đạt được theo yêu cầu: 
  - Yêu cầu thường: 6.75/7đ
  - Yêu cầu nâng cao: 4.5đ
- Điểm mong muốn: 10/10
- Đánh giá: Phân chia các công việc giữa các thành viên tương đối đều đặn, các thành viên đều thực hiện đầy đủ các cong việc
  - Trương Gia Huy - 20127040: 10/10đ
  - Lưu Hoàng Mình - 20127045: 10/10đ
  - Nguyễn Ngọc Quang - 2012297: 10/10đ
  - Hà Tuấn Lâm - 20127677: 10/10đ
## Demo Link
https://youtu.be/yq2IuqZfB9Y
